using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using WebInterface.DTOs;
using WebInterface.Models;
using WebInterface.Services;

namespace WebInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Settings _settings;
        private readonly IPortfolioCalculationService _portfolioCalculationService;
        private readonly IFileParseService _fileParseService;
        private readonly IMemoryCache _cache;

        public HomeController(IFileParseService fileParseService, IPortfolioCalculationService portfolioCalculationService, IMemoryCache cache, IOptions<Settings> settings, ILogger<HomeController> logger)
        {
            _fileParseService = fileParseService;
            _portfolioCalculationService = portfolioCalculationService;
            _cache = cache;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            PortfolioStatus? data = null;
            List<CryptoAsset>? assets;
            bool assetsUploaded = _cache.TryGetValue("CryptoData", out assets);
            if (assetsUploaded && assets != null)
            {
                data = await _portfolioCalculationService.CalculatePortfolio(assets);
                int refreshAfterSeconds = _settings.RefreshIntervalInMinutes * 60;
                Response.Headers.Append("Refresh", refreshAfterSeconds.ToString());
            }

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile fileForUpload)
        {
            var cryptoAssets = await _fileParseService.ParseFileAsync(fileForUpload);
            if (cryptoAssets.Count > 0)
            {
                _cache.Set("CryptoData", cryptoAssets);
                ViewBag.Message = "File Upload Successful";
            }
            else
            {
                ViewBag.Message = "File Upload Failed";
            }

            Response.Headers.Append("Refresh", "1");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
