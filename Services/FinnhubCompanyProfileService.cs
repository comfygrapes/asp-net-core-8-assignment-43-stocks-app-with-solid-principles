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
    public class FinnhubCompanyProfileService : IFinnhubCompanyProfileService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubCompanyProfileService> _logger;

        public FinnhubCompanyProfileService(IFinnhubRepository finnhubRepository, ILogger<FinnhubCompanyProfileService> logger)
        {
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            _logger.LogInformation("Retrieving company profile.");

            return await _finnhubRepository.GetCompanyProfile(stockSymbol);
        }
    }
}
