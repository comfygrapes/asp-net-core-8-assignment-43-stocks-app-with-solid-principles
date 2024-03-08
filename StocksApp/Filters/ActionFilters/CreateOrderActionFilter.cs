using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTOs;
using StocksApp.Controllers;
using StocksApp.ViewModels;

namespace StocksApp.Filters.ActionFilters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is TradeController tradeController)
            {
                if (!tradeController.ModelState.IsValid && context.ActionArguments.TryGetValue("orderRequest", out var retrievedOrderRequest))
                {
                    var stockTrade = new StockTrade();
                    if (retrievedOrderRequest is BuyOrderRequest buyOrderRequest)
                    {
                        stockTrade.StockSymbol = buyOrderRequest.StockSymbol;
                        stockTrade.StockName = buyOrderRequest.StockName;
                        stockTrade.Price = buyOrderRequest.Price;
                        stockTrade.Quantity = buyOrderRequest.Quantity;
                    }
                    else if (retrievedOrderRequest is SellOrderRequest sellOrderRequest) 
                    {
                        stockTrade.StockSymbol = sellOrderRequest.StockSymbol;
                        stockTrade.StockName = sellOrderRequest.StockName;
                        stockTrade.Price = sellOrderRequest.Price;
                        stockTrade.Quantity = sellOrderRequest.Quantity;
                    }
                    tradeController.ViewData["Errors"] = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    context.Result = tradeController.View("Index", stockTrade);
                }
                else
                {
                    await next();
                }
            }
        }
    }
}
