using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace StocksApp.Components
{
    public class SelectedStockViewComponent : ViewComponent
    {
        private readonly IFinnhubCompanyProfileService _finnhubCompanyProfileService;
        private readonly IFinnhubStockPriceQuoteService _finnhubStockPriceQuoteService;

        public SelectedStockViewComponent(IFinnhubCompanyProfileService finnhubCompanyProfileService, IFinnhubStockPriceQuoteService finnhubStockPriceQuoteService)
        {
            _finnhubCompanyProfileService = finnhubCompanyProfileService;
            _finnhubStockPriceQuoteService = finnhubStockPriceQuoteService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string stockSymbol)
        {
            var companyProfile = await _finnhubCompanyProfileService.GetCompanyProfile(stockSymbol) ?? new();
            var stock = await _finnhubStockPriceQuoteService.GetStockPriceQuote(stockSymbol) ?? new();

            
            if (stock.TryGetValue("c", out var currentPrice))
            {
                companyProfile.Add("price", currentPrice);
            }

            return View(companyProfile);
        }
    }
}
