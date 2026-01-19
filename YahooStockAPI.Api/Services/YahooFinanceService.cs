using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using YahooStockAPI.Api.Helpers;
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
    public List<long>? Volumes { get; set; }
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
        // if error, then return a response, log it, and then throw an exception
        var response = await _httpClient.GetAsync($"{baseUrl}/v8/finance/chart/{symbol}?range={range}&interval={interval}");
        _logger.LogInformation("Response status code: {StatusCode}", response.StatusCode);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to retrieve data for symbol: {Symbol}", symbol);
            throw new HttpRequestException("Failed to retrieve data for symbol: " + symbol);
        }

        var json = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(json);
        return doc.RootElement;
    }

    // Creates dictionary of data mapped to a day for simple calculations
    private Dictionary<String, RawDayData> parseRawData(JsonElement rawData)
    {
        Dictionary<String, RawDayData> parsedData = new Dictionary<String, RawDayData>();

        if(!rawData.GetProperty("chart").GetProperty("result")[0].TryGetProperty("timestamp", out _))
        {
            return parsedData;
        }
        var timestamps = rawData.GetProperty("chart").GetProperty("result")[0].GetProperty("timestamp");
        var highs = rawData.GetProperty("chart").GetProperty("result")[0].GetProperty("indicators").GetProperty("quote")[0].GetProperty("high");
        var lows = rawData.GetProperty("chart").GetProperty("result")[0].GetProperty("indicators").GetProperty("quote")[0].GetProperty("low");
        var volumes = rawData.GetProperty("chart").GetProperty("result")[0].GetProperty("indicators").GetProperty("quote")[0].GetProperty("volume");
        
        List<String> days = new List<String>();
        foreach(var timestamp in timestamps.EnumerateArray())
        {
            // Convert each one into a key that makes it easier to group by day
            days.Add(YahooFinanceHelper.ConvertDateTimeToString(YahooFinanceHelper.ConvertToDateTime(timestamp.GetInt64()).Date));
        }
        for(int i = 0; i < days.Count; i++)
        {
            // populate RawDayData lists for calculations
            // Skip empty or null datapoints and don't create unnecessary days
            if (lows[i].ValueKind == JsonValueKind.Null ||
                highs[i].ValueKind == JsonValueKind.Null ||
                volumes[i].ValueKind == JsonValueKind.Null)
            {
                _logger.LogWarning("Skipping null datapoint at index {Index}", i);
                continue;
    }
            double low = lows[i].GetDouble();
            double high = highs[i].GetDouble();
            long volume = volumes[i].GetInt64();

            // Only create new day entry if it already doesn't exist
            if(!parsedData.ContainsKey(days[i]))
            parsedData.Add(days[i], new RawDayData{
                Lows = new List<double>(),
                Highs = new List<double>(),
                Volumes = new List<long>()
            });
            parsedData[days[i]].Lows!.Add(low);
            parsedData[days[i]].Highs!.Add(high);
            parsedData[days[i]].Volumes!.Add(volume);
            
        }
        return parsedData;
    }

    // Returns the final output of a list of Intraday DTOs
    private List<Intraday> calculateAndFormatIntradays(Dictionary<String, RawDayData> parsedData)
    {
        // Convert the DateTime objects to appropriate format YYYY-MM-DD
        List<Intraday> intradayList = new List<Intraday>();

        // Iterate through the dictionary and convert it to a list of Intraday DTOs
        foreach(var value in parsedData)
        {
            var day = value.Key;
            var lowAverage = YahooFinanceHelper.RoundToFourDecimalPlaces(YahooFinanceHelper.CalculateAverage(value.Value.Lows!));
            var highAverage = YahooFinanceHelper.RoundToFourDecimalPlaces(YahooFinanceHelper.CalculateAverage(value.Value.Highs!));
            var volume = value.Value.Volumes!.Sum();
            intradayList.Add(new Intraday{
                Day = day,
                LowAverage = lowAverage,
                HighAverage = highAverage,
                Volume = volume
            });
        }
        return intradayList;
    }

    // Runs the whole api endpoint logic
    // Returns list of Intradays for given symbol
    public async Task<IEnumerable<Intraday>> GetIntradayList(String symbol) {
        // Allows exceptions to pass and the controller to handle them

        // If symbol somehow ends up being empty here, throw exception
        if(string.IsNullOrWhiteSpace(symbol))
        {
            throw new Exception("Symbol is required");
        }
        // default rage of 1 month and interval of 15 minutes
        var rawData = await queryYahooFinanceAsync(symbol, "1mo", "15m");
        var parsedData = parseRawData(rawData);
        var intradayList = calculateAndFormatIntradays(parsedData);
        return intradayList;
    }
}
