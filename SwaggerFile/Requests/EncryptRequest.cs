using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace SwaggerFile.Requests
{
    public class EncrypRequest : SecurityBaseRequest
    {
        //[Required]
        //[FromQuery(Name = "expiration_date")]
        //public long ExpirationDate { get; set; }

        //[Required]
        //[FromQuery(Name = "id")]
        //public string ContractVersionId { get; set; }

        //[Required]
        //[FromQuery(Name = "creationTimestamp")]
        //public long CreationTimestamp { get; set; }

        //[JsonIgnore]
        //public string GetPlainText => $"expiration_date={ExpirationDate}&id={ContractVersionId}&creationTimestamp={CreationTimestamp}&public_key={PublicKey}&private_key={PrivateKey}";

        //public override SortedDictionary<string, string> GetParams()
        //{
        //    var result = new SortedDictionary<string, string>();
        //    foreach (var prop in GetType().GetProperties())
        //    {
        //        result.Add(prop.Name, prop.GetValue(this).ToString());
        //    }
        //    return result;

        //    //
        //    //{
        //    //    {"To", To },
        //    //    {"AccountSid", AccountSid },
        //    //    {"ApiVersion", ApiVersion },
        //    //    {"MessageStatus", MessageStatus },
        //    //    {"SmsSid", SmsSid },
        //    //    {"MessagingServiceSid", MessagingServiceSid },
        //    //    {"From", From },
        //    //    {"MessageSid", MessageSid },
        //    //    {"SmsStatus", SmsStatus },
        //    //};
        //}
    }

    public class EncryptResponse
    {
        public string Data { get; set; }
    }


    public class DecrypRequest
    {
        public string Data { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}