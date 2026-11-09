using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace SwaggerFile.Processor
{
    public class CsvProcessor
    {
        // ---------- Parsing CSV partagé ----------

        public static List<QueueBindingConfig> ParseCsv(string? csvPath = null, string? csvContent = null)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                MissingFieldFound = null,
                BadDataFound = null,
                PrepareHeaderForMatch = args => args.Header.ToLowerInvariant()
            };

            TextReader? r = null;

            if (csvContent != null)
            {
                r = new StringReader(csvContent);
            }
            else if (csvPath != null)
            {
                r = new StreamReader(csvPath);
            }

            if (r == null) throw new Exception("Erreur durant la lecture du fichier de configuration");

            using var reader = r;
            using var csv = new CsvReader(reader, csvConfig);
            return csv.GetRecords<QueueBindingConfig>().ToList();
        }
    }
}
