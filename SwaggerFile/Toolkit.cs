using System.Text;

namespace SwaggerFile
{
    public static class Toolkit
    {
        public static async Task<string> ReadAndStoreFile(HttpContext http)
        {
            var csvContent = await new StreamReader(http.Request.Body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(csvContent)) throw new ArgumentException("file cannot be null");
            var fileName = $"Export_{DateTime.Now:yyyyMMdd_HHmmss_fff}.csv";
            await File.WriteAllTextAsync(fileName, csvContent, Encoding.UTF8, CancellationToken.None);
            return fileName;
        }

        public static async Task<string> Read(HttpContext http)
        {
            var csvContent = await new StreamReader(http.Request.Body).ReadToEndAsync();
            if (string.IsNullOrWhiteSpace(csvContent)) throw new ArgumentException("file cannot be null");
            return csvContent;
        }
    }
}
