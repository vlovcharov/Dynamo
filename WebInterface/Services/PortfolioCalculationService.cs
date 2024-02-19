using WebInterface.DTOs;
using WebInterface.Models;

namespace WebInterface.Services
{
    public class PortfolioCalculationService : IPortfolioCalculationService
    {
        private readonly ICoinLoreService _coinLoreService;
        private readonly ILogger<PortfolioCalculationService> _logger;

        public PortfolioCalculationService(ICoinLoreService coinLoreService, ILogger<PortfolioCalculationService> logger)
        {
            _coinLoreService = coinLoreService;
            _logger = logger;
        }

        public async Task<PortfolioStatus> CalculatePortfolio(List<CryptoAsset> assets)
        {
            PortfolioStatus portfolioStatus = new PortfolioStatus();
            var tickersList = await _coinLoreService.GetTickers();
            if (tickersList != null)
            {
                var assetsList = GetPortfolioCryptoAssetList(tickersList, assets);
                if (assetsList.Count > 0)
                {
                    portfolioStatus.PortfolioCryptoAssets = assetsList;
                    portfolioStatus.InitialValue = portfolioStatus.PortfolioCryptoAssets.Sum(pca => pca.InitialValue);
                    portfolioStatus.CurrentValue = portfolioStatus.PortfolioCryptoAssets.Sum(pca => pca.CurrentValue);
                    portfolioStatus.ValueChangeInPercents = CalculateValueChangeInPercents(portfolioStatus.CurrentValue, portfolioStatus.InitialValue);
                    portfolioStatus.Timestamp = DateTime.Now;
                    return portfolioStatus;
                }
            }

            return new PortfolioStatus();
        }

        private List<PortfolioCryptoAsset> GetPortfolioCryptoAssetList(List<Ticker> tickersList, List<CryptoAsset> assets)
        {
            bool success = false;
            decimal currentPrice = 0;


            List<PortfolioCryptoAsset> result = new List<PortfolioCryptoAsset>();

            foreach (CryptoAsset asset in assets)
            {
                var ticker = tickersList.FirstOrDefault(t => t.Symbol == asset.Name);
                if (ticker != null)
                {
                    success = decimal.TryParse(ticker.Price_Usd, out currentPrice);
                    if (success)
                    {
                        var portfolioCryptoAsset = new PortfolioCryptoAsset();
                        portfolioCryptoAsset.Name = asset.Name;
                        portfolioCryptoAsset.InitialValue = asset.PurchasePrice * asset.HaveQty;
                        portfolioCryptoAsset.CurrentValue = asset.HaveQty * currentPrice;
                        portfolioCryptoAsset.ChangeInPercents = CalculateValueChangeInPercents(portfolioCryptoAsset.CurrentValue, portfolioCryptoAsset.InitialValue);
                        result.Add(portfolioCryptoAsset);
                    }

                }
                else
                {
                    _logger.LogError($"Can't find ticker for asset {asset.Name}");
                    return new List<PortfolioCryptoAsset>();
                }
            }

            return result;
        }


        private decimal CalculateValueChangeInPercents(decimal currentValue, decimal initialValue)
        {
            return ((currentValue / initialValue) * 100) - 100;
        }
    }
}
