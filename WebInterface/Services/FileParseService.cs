using WebInterface.DTOs;

namespace WebInterface.Services
{
    public class FileParseService : IFileParseService
    {
        private readonly ILogger<FileParseService> _logger;

        public FileParseService(ILogger<FileParseService> logger)
        {
            _logger = logger;
        }

        public async Task<List<CryptoAsset>> ParseFileAsync(IFormFile file)
        {
            List<CryptoAsset> result = new List<CryptoAsset>();

            try
            {
                _logger.LogInformation("Parsing uploaded data...");
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                    {
                        var line = await reader.ReadLineAsync();
                        if (line == null) continue;
                        var cryptoAsset = GetCryptoAssetFromLine(line);
                        if (cryptoAsset != null)
                        {
                            result.Add(cryptoAsset);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Parsing uploaded data failed!");
            }

            return result;
        }

        private CryptoAsset? GetCryptoAssetFromLine(string line)
        {
            CryptoAsset? cryptoAsset = null;
            var lineParts = line.Split('|');
            if (lineParts.Length == 3)
            {
                cryptoAsset = new CryptoAsset();
                decimal buffer = 0;

                bool success = decimal.TryParse(lineParts[0].Trim(), out buffer);
                if (!success) return null;
                cryptoAsset.HaveQty = buffer;

                success = decimal.TryParse(lineParts[2].Trim(), out buffer);
                if (!success) return null;
                cryptoAsset.PurchasePrice = buffer;

                cryptoAsset.Name = lineParts[1].Trim();
            }

            return cryptoAsset;
        }

    }
}
