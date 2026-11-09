using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using SwaggerFile.Models;
using SwaggerFile.Processor;

namespace SwaggerFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMqController(ILogger<RabbitMqController> logger, IConfiguration config) : ControllerBase
    {
        private readonly ILogger<RabbitMqController> _logger = logger;
        
        private readonly Dictionary<Environnement, EnvironnementConfig> _configurations =
            new()
            {
                {
                    Environnement.Dev,
                    new EnvironnementConfig
                    {
                        RmqUri = new Uri("https://dev-rabbitmq-hbx-ariane-ocp-rec.ocp-hbx.pragmatik.fr"),
                        RmqLogin = "ariane-rabbitmq",
                        RmqPassword = "aEZX4tMet5BI1HUApgcvPWGZ8x2oRL"
                    }
                },
                {
                    Environnement.Rec,
                    new EnvironnementConfig
                    {
                        RmqUri = new Uri("https://rec-rabbitmq-hbx-ariane-ocp-rec.ocp-hbx.pragmatik.fr"),
                        RmqLogin = "ariane-rabbitmq",
                        RmqPassword = "ThyK5BI1HUApgcvPWGZ8x2Ui"
                    }
                },
                {
                    Environnement.Prod,
                    new EnvironnementConfig
                    {
                        RmqUri = new Uri("https://rabbitmq-hbx-ariane-ocp-prd.ocp-hbx.pragmatik.fr"),
                        RmqLogin = "ariane-rabbitmq",
                        RmqPassword = "LXhcmlhbaGJ4ByZml0bXEKC1mUtcmFiY"
                    }
                }
                ,
                {
                    Environnement.Ppr,
                    new EnvironnementConfig
                    {
                        RmqUri = new Uri("https://ppr-rabbitmq-hbx-ariane-ocp-prd.ocp-hbx.pragmatik.fr"),
                        RmqLogin = "ariane-rabbitmq",
                        RmqPassword = "WLKhhCc2glUCo2OUCsaytkwHp5WpquCp"
                    }
                }
            };

        [HttpPost("ProcessRabbitMQConfiguration")]
        public async Task<IActionResult> ProcessRabbitMQConfiguration([FromQuery]Environnement env)
        {

            var csvContent = await Toolkit.Read(HttpContext);
            var config = _configurations[env];

            var uri = config.RmqUri;
            var login = config.RmqLogin;
            var password = config.RmqPassword;


            var options = new RestClientOptions(uri)
            {
                Authenticator = new HttpBasicAuthenticator(login, password)
            };

            var rows = CsvProcessor.ParseCsv(null, csvContent);

            using var restClient = new RestClient(options);
            var service = new RabbitMqProcessor();
            await service.ConfigureRmqFromCsvAsync(
                restClient,
                vhost: "/",
                csvPath:null,
                csvContent,
                rows
            );

            return Ok();
        }
       
    }
}
