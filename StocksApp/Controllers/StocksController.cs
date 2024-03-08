using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp.Options;
using System.Linq;

namespace StocksApp.Controllers
{
    [Route("[Controller]")]
    public class StocksController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly ILogger<StocksController> _logger;

        public StocksController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, ILogger<StocksController> logger)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
            _logger = logger;
        }

        [HttpGet]
        [Route("/")]
        [Route("[Action]")]
        public async Task<IActionResult> Explore(string? stockSymbol, bool showAllStocks = false)
        {
            _logger.LogInformation("Loading Explore Page");
            _logger.LogDebug($"Current Stock Symbol: {stockSymbol}");

            var retrievedStocks = await _finnhubService.GetStocks();
            IEnumerable<Stock> stocks;

            if (!showAllStocks) 
            {
                var stockSymbols = _tradingOptions.Value.Top25PopularStocks?.Split(',') ?? new []{ "MSFT" };

                retrievedStocks = retrievedStocks?.Where(stock => stockSymbols.Contains(stock["symbol"])).ToList() ?? new();

                stocks = retrievedStocks!.Select(retrievedStock => new Stock()
                {
                    StockSymbol = retrievedStock["symbol"],
                    StockName = retrievedStock["description"],
                });
            }
            else
            {
                stocks = retrievedStocks!.Select(retrievedStock => new Stock()
                {
                    StockSymbol = retrievedStock["symbol"],
                    StockName = retrievedStock["description"],
                });
            }

            if (stockSymbol != null)
            {
                ViewData["SelectedStockSymbol"] = stockSymbol;
            }

            return View(stocks);
        }
    }
}
