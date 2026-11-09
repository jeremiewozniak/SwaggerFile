using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SwaggerFile.Requests
{
    public abstract class SecurityBaseRequest
    {
        [Required]
        [FromQuery(Name = "public_key")]
        public string PublicKey { get; set; }

        [Required]
        [FromQuery(Name = "private_key")]
        public string PrivateKey { get; set; }

        //public abstract SortedDictionary<string, string> GetParams();
    }
}