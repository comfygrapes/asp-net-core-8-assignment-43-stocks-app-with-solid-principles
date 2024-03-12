namespace ServiceContracts
{
    public interface IFinnhubStocksService
    {
        Task<List<Dictionary<string, string>>?> GetStocks();
    }
}