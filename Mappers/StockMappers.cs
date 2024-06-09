using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stock)
        {
            return new StockDto 
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                Company = stock.Company,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,
                Comments = stock.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }

        public static Stock FromStockDtoToStock(this CreateStockRequestDto stock)
        {
            return new Stock
            {
                Symbol = stock.Symbol,
                Company = stock.Company,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,
            };
        }
    }
}