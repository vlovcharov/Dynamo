using WebInterface.DTOs;
using WebInterface.Models;

namespace WebInterface.Services
{
    public interface IPortfolioCalculationService
    {
        Task<PortfolioStatus> CalculatePortfolio(List<CryptoAsset> assets);
    }
}
