using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;
        private readonly IFMPService _fmpService;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo, IFMPService fmpService)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
            _fmpService = fmpService;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetUserPortfolio()
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(user);
            return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var user = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);
            if (stock == null)
            {
                stock = await _fmpService.FindStockBySymbolAsync(symbol);
                if (stock == null)
                {
                    return BadRequest($"Stock {symbol} does not exist");
                }
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
            }
            if (stock == null)
            {
                return BadRequest("Stock not found");
            }
            var portfolio = await _portfolioRepo.GetUserPortfolio(user);
            if (portfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            {
                return BadRequest("Stock already exists in portfolio");
            }
            var newPortfolio = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = user.Id,
            };
            if (newPortfolio == null)
            {
                return BadRequest("Failed to create portfolio");
            }
            await _portfolioRepo.CreateAsync(newPortfolio);
            return Created();
        }
        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var portfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            var fillteredStock = portfolio.Where(e => e.Symbol.ToLower() == symbol.ToLower()).ToList();
            if (fillteredStock.Count == 1)
            {
                await _portfolioRepo.DeletePortfolio(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not in your portfolio");
            }
            return Ok();
        }
    }
}