using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SwaggerFile.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SwaggerFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JwtController : ControllerBase
    {
        [HttpPost("/token")]
        public IActionResult  Login()
        {
            var token = GenerateJwtToken();
            return Ok(new { token });
        }

        private string GenerateJwtToken()
        {
            var key = new SymmetricSecurityKey("your_secret_key"u8.ToArray());
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://services-ariane-dev.homebox.fr/keycloak/realms/ARIANE",
                audience: "ariane-test-restricted",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddSeconds(10800),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("/getjwtpayload")]
        public string GetJwtPayload(string token)
        {

            //read token
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token.Replace("Bearer ", ""));

            return jwtSecurityToken.Payload.SerializeToJson();
        }
    }
}
