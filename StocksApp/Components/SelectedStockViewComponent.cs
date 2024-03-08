using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace StocksApp.Components
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubService _finnhubService;

        public SelectedStockViewComponent(IFinnhubService finnhubService)
        {
            _finnhubService = finnhubService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
        {
            var companyProfile = await _finnhubService.GetCompanyProfile(stockSymbol) ?? new();
            var stock = await _finnhubService.GetStockPriceQuote(stockSymbol) ?? new();

            
            if (stock.TryGetValue("c", out var currentPrice))
            {
                companyProfile.Add("price", currentPrice);
            }

            return View(companyProfile);
        }
    }
}
