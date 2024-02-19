namespace WebInterface.Models
{
    public class PortfolioStatus
    {
        public decimal InitialValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal ValueChangeInPercents { get; set; }
        public List<PortfolioCryptoAsset> PortfolioCryptoAssets { get; set; }
        public DateTime Timestamp { get; set; }

        public PortfolioStatus()
        {
            PortfolioCryptoAssets = new List<PortfolioCryptoAsset>();
        }
    }
}
