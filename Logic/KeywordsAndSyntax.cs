using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    ///     Describe the general syntax for the current popular dotnet version of async programming the Task-based Asynchronous Pattern (TAP)
    /// </summary>
    public class KeywordsAndSyntax
    {
        /// <summary>
        ///     
        /// </summary>
        /// <keyword>
        ///     Async : specifies a method is asynchronous
        ///     Await: inserts a suspension point in the code until the awaited task is completed
        ///     Task: represents an async method
        ///         void: void as return type should be only limited for event handlers.
        ///         Task: Task does not return a response
        ///         Task<T>: returns a values of type T
        ///     Task.Run: request to run on a separate thread
        ///     Task.WhenAny:
        ///     Task.WhenAll: 
        /// </keyword>
        /// <general>
        ///     some libraries that support async methods in C# HttpClient, StreamWriter, StreamReader
        /// </general>
        /// <returns></returns>
        public async Task Example()
        {
            await Task.Run(() => { });
        }
    }
}