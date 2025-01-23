using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(x => x.ToDto()).ToList()
            };
        }
        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap
            };
        }
        public static Stock ToStockFromFMBStock(this FMPStock fmbStock)
        {
            return new Stock
            {
                Symbol = fmbStock.symbol,
                CompanyName = fmbStock.companyName,
                Purchase = (decimal)fmbStock.price,
                LastDiv = (decimal)fmbStock.lastDiv,
                Industry = fmbStock.industry,
                MarketCap = fmbStock.mktCap
            };
        }
    }
}