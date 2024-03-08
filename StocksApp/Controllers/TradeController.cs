using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Options;
using ServiceContracts;
using StocksApp.ViewModels;
using ServiceContracts.DTOs;
using Rotativa.AspNetCore;
using StocksApp.Filters.ActionFilters;

namespace StocksApp.Controllers
{
    [Route("[Controller]")]
    public class TradeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;
        private readonly IStocksService _stocksService;
        private readonly TradingOptions _tradingOptions;
        private readonly ILogger<TradeController> _logger;

        public TradeController(IFinnhubService myService, IConfiguration configuration, IStocksService stocksService, IOptions<TradingOptions> tradingOptions, ILogger<TradeController> logger)
        {
            _finnhubService = myService;
            _configuration = configuration;
            _stocksService = stocksService;
            _tradingOptions = tradingOptions.Value;
            _logger = logger;
        }

        [HttpGet]
        [Route("[Action]/{stockSymbol}")]
        [Route("~/[controller]/{stockSymbol}")]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            _logger.LogInformation($"Loading Trade/Index with stock symbol: {stockSymbol}");

            if (string.IsNullOrEmpty(stockSymbol))
            {
                stockSymbol = "MSFT";
            }

            var company = await _finnhubService.GetCompanyProfile(stockSymbol);
            var stock = await _finnhubService.GetStockPriceQuote(stockSymbol);

            var stockTrade = new StockTrade() { StockSymbol = stockSymbol };
            
            if (stock != null && company != null)
            {
                stockTrade = new StockTrade() 
                { 
                    StockSymbol = company["ticker"].ToString(), 
                    StockName = company["name"].ToString(), 
                    Quantity = Convert.ToUInt32(_tradingOptions.DefaultOrderQuantity), 
                    Price = Convert.ToDouble(stock["c"].ToString()) 
                };
            }

            ViewData["ApiKey"] = _configuration["FinnhubToken"];
            return View(stockTrade);
        }

        [HttpGet]
        [Route("[Action]")]
        public async Task<IActionResult> Orders()
        {
            var buyOrders = await _stocksService.GetAllBuyOrders();
            var sellOrders = await _stocksService.GetAllSellOrders();

            var orders = new Orders()
            {
                BuyOrders = buyOrders,
                SellOrders = sellOrders,
            };

            return View(orders);
        }

        [HttpGet]
        [Route("[Action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            var buyOrders = await _stocksService.GetAllBuyOrders();
            var sellOrders = await _stocksService.GetAllSellOrders();

            var orders = ((IEnumerable<IOrderResponse>)buyOrders).Concat(sellOrders).OrderByDescending(order => order.DateAndTimeOfOrder).ToList();

            return new ViewAsPdf("OrdersPDF", orders)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Bottom = 20, Left = 20, Right = 20 },
            };
        }

        [HttpPost]
        [Route("[Action]")]
        [TypeFilter(typeof(CreateOrderActionFilter))]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest orderRequest)
        {
            ModelState.Remove("DateAndTimeOfOrder");
            if (ModelState.IsValid)
            {
                orderRequest.DateAndTimeOfOrder = DateTime.Now;

                await _stocksService.CreateBuyOrder(orderRequest);

                return RedirectToAction("Orders", "Trade");
            }

            return RedirectToAction("Index", "Trade");
        }


        [HttpPost]
        [Route("[Action]")]
        [TypeFilter(typeof(CreateOrderActionFilter))]   
        public async Task<IActionResult> SellOrder(SellOrderRequest orderRequest)
        {
            ModelState.Remove("DateAndTimeOfOrder");
            if (ModelState.IsValid)
            {
                orderRequest.DateAndTimeOfOrder = DateTime.Now;

                await _stocksService.CreateSellOrder(orderRequest);

                return RedirectToAction("Orders", "Trade"); 
            }

            return RedirectToAction("Index", "Trade");
        }
    }
}
