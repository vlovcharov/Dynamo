using WebInterface.DTOs;

namespace WebInterface.Services
{
    public interface ICoinLoreService
    {
        Task<List<Ticker>> GetTickers();
    }
}
