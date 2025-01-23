using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Newtonsoft.Json;

namespace api.Service
{
    public class FMPService : IFMPService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public FMPService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try
            {
                var endpoint = $"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_configuration["FMPKey"]}";
                Console.WriteLine($"Calling FMP API {endpoint}");
                var result = await _httpClient.GetAsync(endpoint);
                if (result.IsSuccessStatusCode)
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<List<FMPStock>>(content);
                    var stockFMB = tasks?.FirstOrDefault();
                    if (stockFMB != null)
                    {
                        return stockFMB.ToStockFromFMBStock();
                    }
                    Console.WriteLine($"StockFMB {symbol} not found");
                    return null;
                }
                Console.WriteLine($"Error with FMP API {result.StatusCode}");
                return null;
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}