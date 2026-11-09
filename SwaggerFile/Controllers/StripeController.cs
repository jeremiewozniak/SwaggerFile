using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace SwaggerFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StripeSecurityController : ControllerBase
    {
        [HttpPost("validatecallbackrequest")]
        public async Task<ActionResult<bool>> ValidateCallBackRequest() //[FromHeader] StripeRequestHeaders headers
        { 
            var rawRequestBody = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            //var rawRequestBody = await Request.GetRawBodyAsync();

            var signature = Request.Headers["Stripe-Signature"].ToString();
            //signature ="t=1769093443,v1=63fe9cbaa2a0134ecdda4b794cef68818f4959845e3480b61eb5071126018629,v0=4a52d32b3f2927e159296c7606ad4ea523221b1a46f29ed8f0aac310c70a7c1b";

            // rawRequestBody = System.IO.File.ReadAllText("payload.txt", Encoding.UTF8);
            //if (!rawRequestBody.EndsWith("\r\n")) rawRequestBody += "\r\n";
            //var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var isValid = false;
            try
            {
                string endpointSecret = "whsec_oTnvQit1nkrqO5FLriE1rFky7lZeGXZn";
                var stripeEvent = EventUtility.ConstructEvent(rawRequestBody, signature, endpointSecret);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
