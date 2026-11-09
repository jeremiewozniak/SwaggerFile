using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SwaggerFile.Models;

namespace SwaggerFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeycloakRealm : ControllerBase
    {
        [HttpGet("/api/realm")]
        public Root PostComponent()
        {
            var txt = System.IO.File.ReadAllText(@"D:\Downloads\ARIENE-realm-export.json");
            var ariene = JsonConvert.DeserializeObject<Root>(txt);
            txt = System.IO.File.ReadAllText(@"D:\Downloads\ARIAsNE-realm-export.json");
            var ariane = JsonConvert.DeserializeObject<Root>(txt);
            return ariane;
        }
    }
}
