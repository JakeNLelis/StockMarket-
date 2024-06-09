using api.Dtos.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        public Task<List<Stock>> GetAllStocksAsync(QueryObject query);
        public Task<Stock?> GetStockByIdAsync(int id);
        public Task<Stock?> GetStockBySymbolAsync(string symbol);
        public Task<Stock> CreateAsync(Stock stock);
        public Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stock);
        public Task<Stock?> DeleteAsync(int id);
        public Task<bool> StockExist(int id);

    }
}