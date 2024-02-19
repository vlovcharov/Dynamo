using WebInterface.Services;
using WebInterface.DTOs;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTests
{
    public class PortfolioCalculationServiceTests
    {
        [Theory]
        [InlineData(1, 100, 100, 200, 200, 100)]
        [InlineData(1, 100, 100, 300, 300, 200)]
        [InlineData(10, 1000, 10000, 10000, 100000, 900)]
        [InlineData(10, 10000, 100000, 25000, 250000, 150)]
        public async Task ShouldCalculateCorrectValueChangeInPercentsAsync(decimal haveQty, decimal purchasePrice, decimal initialValue, decimal currentPrice, decimal currentValue, decimal valueChangeInPercent)
        {
            Mock<ICoinLoreService> coinLoreServiceMock = new Mock<ICoinLoreService>();
            coinLoreServiceMock.Setup(s => s.GetTickers()).ReturnsAsync(new List<Ticker>()
            {
                new Ticker()
                {
                    Symbol = "BTC",
                    Price_Usd = currentPrice.ToString()
                }
            });

            Mock<ILogger<PortfolioCalculationService>> loggerMock = new Mock<ILogger<PortfolioCalculationService>>();
            var portfolioCalculationService = new PortfolioCalculationService(coinLoreServiceMock.Object, loggerMock.Object);

            List<CryptoAsset> assets = new List<CryptoAsset>()
            {
                new CryptoAsset()
                {
                    HaveQty = haveQty,
                    Name = "BTC",
                    PurchasePrice = purchasePrice
                }
            };

            var portfolio = await portfolioCalculationService.CalculatePortfolio(assets);

            Assert.NotNull(portfolio);
            Assert.Equal(initialValue, portfolio.InitialValue);
            Assert.Equal(currentValue, portfolio.CurrentValue);
            Assert.Equal(valueChangeInPercent, portfolio.ValueChangeInPercents);
            Assert.Equal(1, portfolio.PortfolioCryptoAssets?.Count);
        }
    }
}