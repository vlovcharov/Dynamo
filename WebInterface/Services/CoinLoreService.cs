using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WebInterface.DTOs;

namespace WebInterface.Services
{
    public class CoinLoreService : ICoinLoreService
    {
        private readonly Settings _settings;
        private readonly ILogger<CoinLoreService> _logger;
        public CoinLoreService(IOptions<Settings> settings, ILogger<CoinLoreService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<List<Ticker>> GetTickers()
        {
            List<Ticker> tickers = new List<Ticker>();
            try
            {
                _logger.LogInformation("Getting data from CoinLore API");
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(_settings.CoinLoreApiAddress))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiResponse);
                        if (responseDto != null && responseDto.Data != null)
                        {
                            tickers = responseDto.Data;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Error getting data from CoinLore API:");
            }

            return tickers;
        }
    }
}
