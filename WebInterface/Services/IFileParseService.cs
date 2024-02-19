using WebInterface.DTOs;

namespace WebInterface.Services
{
    public interface IFileParseService
    {
        Task<List<CryptoAsset>> ParseFileAsync(IFormFile file);
    }
}
