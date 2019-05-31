using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ExampleController : Controller
    {
        private readonly IAsyncAwaitExamples _asyncAwaitExamples;

        /// <summary>
        ///     Example async calls
        /// </summary>
        /// <param name="asyncAwaitExamples"></param>
        public ExampleController(IAsyncAwaitExamples asyncAwaitExamples)
        {
            _asyncAwaitExamples = asyncAwaitExamples;
        }

        /// <summary>
        ///     An example of CPU bound asynchronous code
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("CpuBoundAsync")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> CpuBoundAsync()
        {
            return Ok(await _asyncAwaitExamples.CpuBoundAsync());
        }

        /// <summary>
        ///     An example of IO bound async method
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("IoBoundCodeAsync")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> IoBoundCodeAsync()
        {
            await _asyncAwaitExamples.IoBoundCodeAsync();
            return Ok("Success");
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("IoBoundCodeAsync")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ComplexCPUBoundLogicAsnyc()
        {
            await _asyncAwaitExamples.ComplexCPUBoundLogicAsync();
            return Ok("Success");
        }
        
    }
}