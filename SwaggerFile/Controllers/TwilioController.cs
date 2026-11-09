using Microsoft.AspNetCore.Mvc;
using Twilio.Security;

namespace SwaggerFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwilioController : ControllerBase
    {
        private readonly ILogger<TwilioController> _logger;
        private readonly IConfiguration _config;

        public TwilioController(ILogger<TwilioController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpPost("/validaterequest")]
        public bool Post()
        {
            // Your Auth Token from twilio.com/console
            var authToken = _config.GetValue<string>("TWILIO_AUTH_TOKEN");

            // Initialize the validator
            var validator = new RequestValidator(authToken);

            // The Twilio request URL
            const string url = "https://twilio-homebox.free.beeceptor.com";

            // The X-Twilio-Signature header attached to the request
            const string twilioSignature = "FGP0+XZouH2lFJH7oO0oc2WP2Ns=";

            var parameters = new Dictionary<string, string>
        {
                { "SmsSid", "SM1ff19bd069c146d5216c466d7a5e2e0b" },
                { "SmsStatus", "sent" },
                { "MessageStatus", "sent" },
                { "To", "+33625619257" },
                { "MessagingServiceSid", "MGd34548e7205e04218f64c03580cb5d25" },
                { "MessageSid", "SM1ff19bd069c146d5216c466d7a5e2e0b" },
                { "AccountSid", "AC9d1dd05a03cbf7842b895eca0e90b777" },
                { "From", "+16089275171" },
                { "ApiVersion", "2010-04-01" },
        };

            return validator.Validate(url, parameters, twilioSignature);
        }
    }
}
