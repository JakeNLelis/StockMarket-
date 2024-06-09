
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace api.Controllers;

[Route("api/portfolio")]
[ApiController]
[Authorize]
public class PortfolioController : ControllerBase 
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepo;
    private readonly IPortfolioRepository _portfolioRepo;
    public PortfolioController(UserManager<AppUser> userManager,
        IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
    {
        _stockRepo = stockRepo;
        _userManager = userManager;
        _portfolioRepo = portfolioRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserPortfolio () 
    {
        var username = User.GetUsername();

        var appUser =  await _userManager.FindByNameAsync(username);

        if (appUser == null) return BadRequest("Cannot find User");

        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

        return Ok(userPortfolio);
    }

    [HttpPost]
    public async Task<IActionResult> AddPortfolio (string symbol)
    {
        var username = User.GetUsername();
        
        var appUser = await _userManager.FindByNameAsync(username);
        
        var stock = await _stockRepo.GetStockBySymbolAsync(symbol);

        if(stock == null) return BadRequest("Stock not Found");

        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("This stock already exist in your portfolio!"); 

        Portfolio portfolio = new()
        {
            AppUserId = appUser.Id,
            StockId = stock.Id,
        };

        var portfolioResult = await _portfolioRepo.CreateAsync(portfolio);

        if(portfolioResult == null) return StatusCode(500, "Could not create portfolio");

        return Ok(portfolioResult);
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePortfolio (string symbol)
    {
        var username = User.GetUsername();
        
        var appUser = await _userManager.FindByNameAsync(username);

        var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

        var filteredPorfolio = userPortfolio.Where(e => e.Symbol.ToLower() == symbol.ToLower()).ToList();

        if (filteredPorfolio.Count == 1) 
            await _portfolioRepo.DeleteAsync(appUser, symbol);
        else 
            return BadRequest("Stock doesn't exist!");

        return Ok(filteredPorfolio[0].ToStockDto());
    }
}