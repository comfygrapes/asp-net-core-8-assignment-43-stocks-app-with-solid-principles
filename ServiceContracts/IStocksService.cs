using ServiceContracts.DTOs;

namespace ServiceContracts
{
    public interface IStocksService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest);

        Task<List<BuyOrderResponse>> GetAllBuyOrders();

        Task<List<SellOrderResponse>> GetAllSellOrders();
    }
}
