using Microsoft.AspNetCore.Mvc;
using SwaggerFile.Models;
using SwaggerFile.Processor;

namespace SwaggerFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApimController : ControllerBase
    {
        [HttpPost("Test")]
        public async Task<IActionResult> Test([FromQuery] Environnement env)
        {
            try
            {
                var fileName = await Toolkit.ReadAndStoreFile(HttpContext);
                var tester = new RmqDestinationTester(env);
                var dests = RmqDestinationTester.LoadFromCsv(fileName);
                var results = await tester.RunAsync(dests);
                return Ok(results);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}
