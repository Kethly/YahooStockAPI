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
        // Use a default client user agent to avoid rate limit errors
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
        _logger = logger;
    }
    // Queries Yahoo Finance API for stock data with symbol
    // time range and set interval
    // Returns the raw JSON data and checks for errors
    private async Task<JsonElement> queryYahooFinanceAsync(String symbol, String range, String interval) {
        // TODO implement Yahoo Finance API
        // if error, then return a response, log it, and then throw an exception
        _logger.LogInformation("Url: {baseUrl}/v8/finance/chart/{symbol}?range={range}&interval={interval}", baseUrl, symbol, range, interval);
        var response = await _httpClient.GetAsync($"{baseUrl}/v8/finance/chart/{symbol}?range={range}&interval={interval}");
        _logger.LogInformation("Response status code: {StatusCode}", response.StatusCode);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to retrieve data for symbol: {Symbol}", symbol);
            throw new HttpRequestException("Failed to retrieve data for symbol: " + symbol);
        }

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        _logger.LogInformation("raw data: {Json}", json);
        return doc.RootElement;
    }

    // Creates dictionary for simple calculations
    private Dictionary<DateTime, RawDayData> parseRawData(JsonElement rawData)
    {
        // TODO implement parsing logic
        var parsedData = new Dictionary<DateTime, RawDayData>();
        //var highs = rawData.GetProperty("chart").GetProperty("result")[0].GetProperty("indicators").GetProperty("quote")[0].GetProperty("high");
        //var lows = rawData["chart"]["result"][0]["indicators"]["quote"][0]["low"];
        //var volumes = rawData["chart"]["result"][0]["indicators"]["quote"][0]["volume"];
        //var timestamps = rawData["chart"]["result"][0]["timestamp"];
        foreach (var property in rawData.EnumerateObject())
        {
            // populate RawDayData lists for calculations
        }
        return default;
    }

    // Returns the final output of a list of Intradays
    private List<Intraday> calculateAndFormatIntradays(Dictionary<DateTime, RawDayData> parsedData)
    {
        // TODO implement calculations and return list of Intradays
        // Convert the DateTime objects to appropriate format YYYY-MM-DD
        return default;
    }

    // Runs the whole api endpoint logic
    // Returns list of Intradays for given symbol
    public async Task<IEnumerable<Intraday>> GetIntradayList(String symbol) {
        // TODO implement controller logic
        // Allows exceptions to pass and the controller to handle them
        var rawData = await queryYahooFinanceAsync(symbol, "1mo", "15m");
        var parsedData = parseRawData(rawData);
        var intradayList = calculateAndFormatIntradays(parsedData);
        return intradayList;
    }
}
