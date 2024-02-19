namespace WebInterface.Models
{
    public class PortfolioCryptoAsset
    {
        public string? Name { get; set; }
        public decimal InitialValue { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal ChangeInPercents { get; set; }
    }
}
