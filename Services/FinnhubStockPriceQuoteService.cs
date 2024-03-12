using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FinnhubStockPriceQuoteService : IFinnhubStockPriceQuoteService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubStockPriceQuoteService> _logger;

        public FinnhubStockPriceQuoteService(IFinnhubRepository finnhubRepository, ILogger<FinnhubStockPriceQuoteService> logger)
        {
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            _logger.LogInformation("Retrieving stock price quote.");

            return await _finnhubRepository.GetStockPriceQuote(stockSymbol);
        }
    }
}
