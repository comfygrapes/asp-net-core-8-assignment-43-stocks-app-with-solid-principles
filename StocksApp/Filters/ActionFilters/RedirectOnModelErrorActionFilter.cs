using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTOs;

namespace StocksApp.Filters.ActionFilters
{
    public class RedirectOnModelErrorActionFilter : IActionFilter
    {
        private readonly ILogger<RedirectOnModelErrorActionFilter> _logger;

        public RedirectOnModelErrorActionFilter(ILogger<RedirectOnModelErrorActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("On Executed");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("On Executing");
            if (!context.ModelState.IsValid && context.ActionArguments.TryGetValue("orderRequest", out var retrievedOrderRequest))
            {
                var stockSymbol = "";
                if (retrievedOrderRequest is BuyOrderRequest buyOrderRequest)
                {
                    stockSymbol = buyOrderRequest.StockSymbol;
                }
                else if (retrievedOrderRequest is SellOrderRequest sellOrderRequest)
                {
                    stockSymbol = sellOrderRequest.StockSymbol;
                }

                context.Result = new RedirectToActionResult("Index", "Trade", new { stockSymbol });
            }
        }
    }
}
