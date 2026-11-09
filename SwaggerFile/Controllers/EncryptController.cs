using Microsoft.AspNetCore.Mvc;
using SwaggerFile.Requests;
using System.Security.Cryptography;
using System.Text;

namespace SwaggerFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EncryptController(ILogger<EncryptController> logger, IConfiguration config) : ControllerBase
    {

        private readonly ILogger<EncryptController> _logger = logger;
        private readonly IConfiguration _config = config;

        [HttpGet("encrypt")]
        public async Task<ActionResult<EncryptResponse>> EncryptData()
        {
            //var plaintext = query.GetPlainText;
            var guidPublicKey = Guid.NewGuid().ToString();
            var guidToEncrypt = Guid.NewGuid().ToString();

            var (publickey, secretKey) = GenerateKeys(8);

            GenerateKeys(publickey, secretKey, out var publicBytes, out var privateBytes);

            // Encrypt
            var ciphertext = AesEncryption.Encrypt(guidToEncrypt, publicBytes, privateBytes);
            var encryptedText = Convert.ToBase64String(ciphertext);
            return new EncryptResponse
            {
                Data = encryptedText
            };
        }

        private static void GenerateKeys(string guidPublicKey, string privateKeyStr, out byte[] publicKeyBytes, out byte[] privateKeyBytes)
        {
            // Generate a key and IV
            publicKeyBytes = new byte[32];
            privateKeyBytes = new byte[16];
            privateKeyBytes = Encoding.UTF8.GetBytes(privateKeyStr);
            publicKeyBytes = Encoding.UTF8.GetBytes(guidPublicKey.Replace("-", string.Empty));
        }

        public static (string SecretKey, string PublicKey) GenerateKeys(int secretKeySizeInBytes)
        {
            // Public Key : GUID
            string publicKey = Guid.NewGuid().ToString();//.Replace("-", string.Empty);

            // Secret Key : généré en fonction de la taille demandée
            byte[] secretKeyBuffer = new byte[secretKeySizeInBytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(secretKeyBuffer);
            }

            // Conversion en chaîne Hexadécimale
            string secretKey = BitConverter.ToString(secretKeyBuffer).Replace("-", string.Empty);

            return (publicKey, secretKey);
        }

        [HttpGet("decrypt")]
        public async Task<ActionResult<EncryptResponse>> DecryptData([FromQuery] DecrypRequest query)
        {
            var plaintext = query.Data;
            GenerateKeys(query.PublicKey, query.PrivateKey, out var privateKey, out var iv);

            // Decrypt
            var bytes = Convert.FromBase64String(plaintext);
            var decryptedText = AesEncryption.Decrypt(bytes, privateKey, iv);
            return new EncryptResponse
            {
                Data = decryptedText
            };
        }




        internal class AesEncryption
        {
            public static byte[] Encrypt(string plaintext, byte[] key, byte[] iv)
            {
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;
                    var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    byte[] encryptedBytes;
                    using (var msEncrypt = new System.IO.MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            var plainBytes = Encoding.UTF8.GetBytes(plaintext);
                            csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                        }
                        encryptedBytes = msEncrypt.ToArray();
                    }
                    return encryptedBytes;
                }
            }
            public static string Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
            {
                using (var aesAlg = Aes.Create())
                {
                    aesAlg.Key = key;
                    aesAlg.IV = iv;
                    var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    byte[] decryptedBytes;
                    using (var msDecrypt = new System.IO.MemoryStream(ciphertext))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var msPlain = new System.IO.MemoryStream())
                            {
                                csDecrypt.CopyTo(msPlain);
                                decryptedBytes = msPlain.ToArray();
                            }
                        }
                    }
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
    }
}