using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Xml.Linq;
using JeppeStaerk.OnePasswordConnect.Sdk;
using Twilio.Jwt.AccessToken;

namespace SwaggerFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SandboxController : ControllerBase
    {
        public SandboxController(OnePasswordConnectClient client) => _client = client;
        private readonly OnePasswordConnectClient _client; 

        [HttpGet("/test")]
        public async Task<ActionResult<string>> Get()
        {
            var t = await _client.Vaults.GetVaultsAsync();
            return Ok();
        }
    }
}