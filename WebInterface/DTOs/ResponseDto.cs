namespace WebInterface.DTOs
{
    public class ResponseDto
    {
        public List<Ticker>? Data { get; set; }
        public ResponseFooter? FooterInfo { get; set; }
    }
}
