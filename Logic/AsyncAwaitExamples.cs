using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Logic.Interfaces;

namespace Logic
{
    /// <summary>
    ///     Understanding how Asnyc Await works under the covers in C#
    /// </summary>
    /// <Benefits>
    ///     By not blocking we can potentially perform additional work
    ///     Additionally because we are returning threads to the thread pool during potentially expensive
    ///     operations we can use the threads to service new web requests while we wait for a response
    /// </Benefits>
    /// <Resources>
    ///     https://www.codeproject.com/Articles/535635/Async-Await-and-the-Generated-StateMachine
    ///     https://stackify.com/csharp-async-await-task-performance/
    ///     https://docs.microsoft.com/en-us/dotnet/standard/async-in-depth
    ///     https://weblogs.asp.net/dixin/understanding-c-sharp-async-await-1-compilation
    ///     https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/how-to-extend-the-async-walkthrough-by-using-task-whenall
    /// </Resources>
    /// <General>
    ///     The compiler creates an asynchronous state machine as a struct on the stack
    ///     When an I/O operation starts, code on that thread ends allowing the thread to be returned
    ///     to the thread pool, examples are calls to external servers, etc that might be long running
    ///     When the I/O operation returns then a new thread begins the work from the await
    ///     It is moved to heap inside the AwaitUnsafeOnCompleted function and deeper still is cast to
    ///     IAsyncStateMachine which triggers boxing
    ///     This call may be skipped if the task we await ends before the taskAwaiter.IsComplete is checked. 
    ///     The operating system performs a wake up call that grabs a new thread from the thread pool  
    /// </General>    
    public class AsyncAwaitExamples : IAsyncAwaitExamples
    {
        private readonly IOtherWork _otherWork;

        public AsyncAwaitExamples(IOtherWork otherWork)
        {
            _otherWork = otherWork;
        }

        /// <summary>
        ///     I/O bound code is an appropriate example of when to use async code.
        ///     Some examples include downloading a large resource from a network, reading a large file or
        ///     accessing a database resource. In this case, you use the await keyword on an async method
        ///     that returns a Task or Task<T>. 
        /// </summary>
        /// <returns></returns>
        public async Task IoBoundCodeAsync()
        {
            // All local variables are pulled into an IAsyncStateMachine as private members in 
            // the Intermediate Language
            HttpClient client = new HttpClient();

            // In the IL code we have helper methods to determine when we should continue execution
            // Some examples of these methods include
            // MoveNext()
            // GetAwaiter()
            // get_IsCompleted()
            // AwaitUnsafeOnCompleted
            // GetResult()
            // SetStateMachine()


            // Allows us to run both tasks concurrently then await both responses
            var taskOne = client.GetAsync("http://localhost:5003/api/Example/CpuBoundAsync");
            var taskTwo = client.GetAsync("http://localhost:5003/api/Example/CpuBoundAsync");

            // Super important other work that needs to run for business reasons
            // To note if any portion of an operation is asynchronous, 
            // regardless of if there is synchronous work the entire operation is asynchronous
            // unless keywords like result are used.
            _otherWork.ImportantWork();

            // Will return await all results, if any task fails then the exception bubbles up causing all tasks to fail 
            await Task.WhenAll(taskOne, taskTwo);
        }

        /// <summary>
        ///     Asynchronous Call to calculate
        ///     CPU bound code. This is when you want to do a heavy in-app calculations.
        ///     In this case we can use the await keyword on an async method that will be running o
        /// n a background thread using Task.Run()
        /// </summary>
        /// <returns></returns>
        public async Task<long> CpuBoundAsync()
        {
            return await Task.Run(() => _otherWork.SlowCalculation(100));
        }


        public async Task<bool> ComplexCPUBoundLogicAsync()
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            Task t;

            // Create a thread safe data structure to contain the tasks that will be executed
            var tasks = new ConcurrentBag<Task>();
            t = Task.Factory.StartNew(() => IntensiveCalculation(token), token);
            tasks.Add(t);
            return true;
        }

        public void IntensiveCalculation(CancellationToken token)
        {
            return;
        }
    }
}