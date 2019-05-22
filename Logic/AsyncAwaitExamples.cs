using System.Collections;
using System.Linq;
using System.Net.Http;
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
    /// <General>
    ///     The compiler under the cover creates an asynchronous state machine as a struct on the stack
    ///     When an I/O operation starts, code on that thread ends allowing the thread to be returned to the thread pool, examples are calls to external servers, etc that might be long running
    ///     When the I/O operation returns then a new thread begins the work from the await
    ///     It is moved to heap inside the AwaitUnsafeOnCompleted function (deeper into the mechanism it is cast to IAsyncStateMachine which triggers the boxing). 
    ///     This call may be skipped if the task we await ends before the taskAwaiter.IsComplete is checked. 
    ///     The operating system performs a wake up call that grabs a new thread from the thread pool  
    /// </General>
    /// <Resources>
    ///     https://www.codeproject.com/Articles/535635/Async-Await-and-the-Generated-StateMachine
    ///     https://stackify.com/csharp-async-await-task-performance/
    ///     https://docs.microsoft.com/en-us/dotnet/standard/async-in-depth
    ///     https://weblogs.asp.net/dixin/understanding-c-sharp-async-await-1-compilation
    ///     https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/how-to-extend-the-async-walkthrough-by-using-task-whenall
    /// </Resources>    
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
            HttpClient client = new HttpClient();

            // Allows us to run both tasks concurrently then await both responses
            var taskOne = client.GetAsync("http://localhost:5003/api/Example/CpuBoundAsync");
            var taskTwo = client.GetAsync("http://localhost:5003/api/Example/CpuBoundAsync");

            // Super important other work that needs to run for business reasons
            // To note if any portion of an operation is asynchronous, the entire operation is asynchronous.
            _otherWork.ImportantWork();

            // Will return await all results, if any task fails then the exception bubbles up causing all tasks to fail 
            await Task.WhenAll(taskOne, taskTwo);
        }

        /// <summary>
        ///     Asynchronous Call to calculate
        ///     CPU bound code. This is when you want to do a heavy in-app calculations.
        ///     In this case we can use the await keyword on an async method that will be running on a background thread using Task.Run()
        /// </summary>
        /// <returns></returns>
        public async Task<long> CpuBoundAsync()
        {
            return await Task.Run(() => _otherWork.SlowCalculation(100));
        }

        // As of C# 8 and dotnetcore 3 the language now supports non scalar return values
        /// <summary>
        ///     To note this feature is only helpful in C# 8 (dotnet core 3) as of the making of this repo it is still in beta
        /// </summary>
        /// <returns></returns>
        /*
        public async Task Example()
        {
            await foreach(var dataPoint in DataSet())
            {
                Console.WriteLine(dataPoint);
            }
        }
 
        static async IAsyncEnumerable<int> DataSet()
        {
            for (int i = 1; i <= 1000; i++)
            {
                await Task.Delay(100);//Simulate waiting for data to come through. 
                yield return i;
            }
        }
        */
    }
}