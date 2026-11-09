using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StrongGrid;
using StrongGrid.Models.Webhooks;
using SwaggerFile.Models;

namespace SwaggerFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendGridController : ControllerBase
    {
        [HttpPost("parseinboundemail")]
        public async Task<ActionResult<InboundEmailResponse>> ParseInboundEmail()
        {
            var parser = new WebhookParser();
            var i = await parser.ParseInboundEmailWebhookAsync(Request.Body);

            return new InboundEmailResponse(i.Attachments)
            {
                Html = i.Html,
                Cc = i.Cc,
                Charsets = i.Charsets.ToLookup(k => k.Key, v => v.Value),
                Dkim = i.Dkim,
                Envelope = i.Envelope,
                From = i.From,
                Headers = i.Headers.ToLookup(k => k.Key, v => v.Value),
                RawEmail = i.RawEmail,
                SenderIp = i.SenderIp,
                SpamReport = i.SpamReport,
                SpamScore = i.SpamScore,
                Spf = i.Spf,
                Subject = i.Subject,
                Text = i.Text,
                To = i.To
            };
        }

        [HttpPost("sg/in")]
        //[Consumes("text/plain", "multipart/form-data", "multipart/form-data; boundary=xYzZY")]
        public async Task<ActionResult<string>> IncomingSendgrid()
        {
            WebhookParser parser = new();
            InboundEmail email = await parser.ParseInboundEmailWebhookAsync(HttpContext.Request.Body);
            Console.WriteLine($"Body.Html recompiled: '{email.Html}'");
            Console.WriteLine($"Body.Text recompiled: '{email.Text}'");
            Console.WriteLine($"HttpContext.Request.ContentType: '{HttpContext.Request.ContentType}'");
            return Ok(email.Html);

        }
    }


}
