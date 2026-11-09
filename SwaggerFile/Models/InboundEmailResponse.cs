using System.Text.Json.Serialization;
using StrongGrid.Models;
using StrongGrid.Models.Webhooks;

namespace SwaggerFile.Models
{
    /// <summary>
    /// The parsed information about an email sent by SendGrid via a webhook.
    /// </summary>
    public class InboundEmailResponse
    {
        public InboundEmailResponse(InboundEmailAttachment[] attachments)
        {
            var myAttachments = new List<InboundEmailAttachmentResponse>();
            foreach (var i in attachments)
            {
                using var ms = new MemoryStream();
                i.Data.CopyTo(ms);
                myAttachments.Add(new InboundEmailAttachmentResponse
                {
                    Name = i.Name,
                    ContentId = i.ContentId,
                    ContentType = i.ContentType,
                    FileName = i.FileName,
                    Id = i.Id,
                    Data = ms.ToArray()
                });
            }
            Attachments = myAttachments.ToArray();
        }

        /// <summary>
        /// Gets or sets the headers of the email.
        /// </summary>
        /// <value>
        /// The headers.
        /// </value>
        public ILookup<string, string> Headers { get; set; }

        /// <summary>
        /// Gets or sets a string containing the verification results of any DKIM and domain keys signatures in the message.
        /// </summary>
        /// <value>
        /// The dkim.
        /// </value>
        public string Dkim { get; set; }

        /// <summary>
        /// Gets or sets the email recipient field, as taken from the message headers.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public MailAddress[] To { get; set; }

        /// <summary>
        /// Gets or sets the carbon copy recipient field, as taken from the message headers.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        public MailAddress[] Cc { get; set; }

        /// <summary>
        /// Gets or sets the HTML body of email. If not set, email did not have a HTML body.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        public string Html { get; set; }

        /// <summary>
        /// Gets or sets the TEXT body of email .If not set, email did not have a TEXT body.
        /// </summary>
        /// <value>
        /// The TEXT.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets from the email sender, as taken from the message headers.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        public MailAddress From { get; set; }

        /// <summary>
        /// Gets or sets the sender's ip address.
        /// </summary>
        /// <value>
        /// The sender ip.
        /// </value>
        public string SenderIp { get; set; }

        /// <summary>
        /// Gets or sets the Spam Assassin's spam report.
        /// </summary>
        /// <value>
        /// The spam report.
        /// </value>
        public string SpamReport { get; set; }

        /// <summary>
        /// Gets or sets the SMTP envelope.
        /// This will have two variables:
        /// - to, which is a single-element array containing the address that we received the email to,
        /// - from, which is the return path for the message.
        /// </summary>
        /// <value>
        /// The envelope.
        /// </value>
        public InboundEmailEnvelope Envelope { get; set; }

        /// <summary>
        /// Gets or sets the email subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the Spam Assassin’s rating for whether or not this is spam.
        /// </summary>
        /// <value>
        /// The spam score.
        /// </value>
        public string SpamScore { get; set; }

        /// <summary>
        /// Gets or sets the attachment.
        /// </summary>
        /// <value>
        /// The attachment.
        /// </value>
        public InboundEmailAttachmentResponse[] Attachments { get; set; }

        /// <summary>
        /// Gets or sets the character sets of the fields extracted from the message.
        /// </summary>
        /// <value>
        /// The charsets.
        /// </value>
        public ILookup<string, string> Charsets { get; set; }

        /// <summary>
        /// Gets or sets the results of the Sender Policy Framework verification of the message sender
        /// and receiving IP address.
        /// </summary>
        /// <value>
        /// The SPF.
        /// </value>
        public string Spf { get; set; }

        /// <summary>
        /// Gets or sets the raw email received.
        /// </summary>
        /// <value>
        /// The raw email.
        /// </value>
        public string RawEmail { get; set; }
    }

    /// <summary>
    /// Strongly typed representation of the information submited by SendGrid in a 'inbound parse' webhook.
    /// </summary>
    public class InboundEmailAttachmentResponse
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the content-type. Defaults to text/plain if unspecified.
        /// </summary>
        /// <value>
        /// The content-type.
        /// </value>
        [JsonPropertyName("type")]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public byte[] Data { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [JsonPropertyName("filename")]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the content identifier.
        /// </summary>
        /// <value>
        /// The content identifier.
        /// </value>
        [JsonPropertyName("content-id")]
        public string ContentId { get; set; }
    }
}
