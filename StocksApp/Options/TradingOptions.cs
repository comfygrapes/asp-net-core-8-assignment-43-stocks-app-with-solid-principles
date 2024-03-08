using Microsoft.Extensions.Options;

namespace StocksApp.Options
{
    public class TradingOptions
    {
        public string? DefaultStockSymbol { get; set; }
        public int? DefaultOrderQuantity { get; set; }
        public string? Top25PopularStocks { get; set; }    
    }
}
