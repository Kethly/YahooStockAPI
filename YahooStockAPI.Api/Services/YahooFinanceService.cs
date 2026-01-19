using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using YahooStockAPI.Models;

namespace YahooStockAPI.Api.Services;

public interface IYahooFinanceService 
{
    // Runs the whole api endpoint logic
    Task<IEnumerable<Intraday>> GetIntradayList(String symbol);

}

// Skeleton class for raw day data for calculations
public class RawDayData
{
    public List<double>? Lows { get; set; }
    public List<double>? Highs { get; set; }
    public List<int>? Volumes { get; set; }
}

public class YahooFinanceService : IYahooFinanceService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<YahooFinanceService> _logger;
    private const string baseUrl = "https://query1.finance.yahoo.com";

    public YahooFinanceService(HttpClient httpClient, ILogger<YahooFinanceService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
    // Queries Yahoo Finance API for stock data with symbol
    // time range and set interval
    // Returns the raw JSON data and checks for errors
    private async Task<JsonElement> queryYahooFinanceAsync(String symbol, String range, String interval) {
        // TODO implement Yahoo Finance API
        // if error, then return a response, log it, and then throw an exception
        return default;
    }

    // Creates dictionary for simple calculations
    private Dictionary<DateTime, RawDayData> parseRawData(JsonElement rawData)
    {
        // TODO implement parsing logic
        return default;
    }

    // Returns the final output of a list of Intradays
    private List<Intraday> formatIntradays(Dictionary<DateTime, RawDayData> parsedData)
    {
        // TODO implement calculations and return list of Intradays
        return default;
    }

    // Runs the whole api endpoint logic
    // Returns list of Intradays for given symbol
    public async Task<IEnumerable<Intraday>> GetIntradayList(String symbol) {
        // TODO implement controller logic
        // Allows exceptions to pass and the controller to handle them
        return default;
    }
}
