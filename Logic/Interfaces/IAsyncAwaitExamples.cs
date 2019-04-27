using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IAsyncAwaitExamples
    {
        /// <summary>
        ///     I/O bound code is an appropriate example of when to use async code.
        ///     Some examples include downloading a large resource from a network, reading a large file or
        ///     accessing a database resource. In this case, you use the await keyword on an async method
        ///     that returns a Task or Task<T>. 
        /// </summary>
        /// <returns></returns>
        Task IoBoundCodeAsync();

        /// <summary>
        ///     Asynchronous Call to calculate
        ///     CPU bound code. This is when you want to do a heavy in-app calculations.
        ///     In this case we can use the await keyword on an async method that will be running on a background thread using Task.Run()
        /// </summary>
        /// <returns></returns>
        Task<long> CpuBoundAsync();
    }
}