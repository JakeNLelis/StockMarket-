using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Dtos.Stock;
using api.Interfaces;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [ApiController]
    [Route("api/stock")]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;

        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStocks([FromQuery] QueryObject query)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var stocks = await _stockRepo.GetAllStocksAsync(query);

            var stockList = stocks.Select(s => s.ToStockDto()).ToList();

            return Ok(stockList);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetStockById([FromRoute] int id)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var stock = await _stockRepo.GetStockByIdAsync(id);

            return stock == null ? NotFound() : Ok(stock.ToStockDto());
        }   
        
        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto data)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var stockModel = data.FromStockDtoToStock();

            await _stockRepo.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetStockById), new {id = stockModel.Id}, stockModel.ToStockDto());
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDto updatedStock)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var stockModel = await _stockRepo.UpdateAsync(id, updatedStock);

            return stockModel == null ? NotFound() : Ok(stockModel.ToStockDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            var stock = await _stockRepo.DeleteAsync(id); 
        
            return stock == null ? NotFound(): NoContent();
        }
    }
}