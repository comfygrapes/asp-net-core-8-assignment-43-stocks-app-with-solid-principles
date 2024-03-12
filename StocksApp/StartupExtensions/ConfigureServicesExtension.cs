using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using ServiceContracts;
using Services;
using StocksApp.Options;

namespace StocksApp.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddDbContext<StockMarketDbContext>((options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")!);
            });

            services.Configure<TradingOptions>(configuration.GetSection("TradingOptions"));
            services.AddScoped<IFinnhubRepository, FinnhubRepository>();
            services.AddScoped<IFinnhubStocksService, FinnhubStocksService>();
            services.AddScoped<IFinnhubCompanyProfileService, FinnhubCompanyProfileService>();
            services.AddScoped<IFinnhubStockPriceQuoteService, FinnhubStockPriceQuoteService>();
            services.AddScoped<IFinnhubSearchStocksService, FinnhubSearchStocksService>();
            services.AddScoped<IStocksRepository, StocksRepository>();
            services.AddScoped<IStocksService, StocksService>();
        }
    }
}
