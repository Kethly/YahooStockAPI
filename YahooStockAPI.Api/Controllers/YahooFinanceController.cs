/* YahooFinanceController
   Exposes REST endpoints for retrieving intraday stock data such as averages and sums.
   Checks input symbols and converts service-layer exceptions into HTTP responses.*/
using Microsoft.AspNetCore.Mvc;
using YahooStockAPI.Api.Services;
using YahooStockAPI.Models;

namespace YahooStockAPI.Api.Controllers;

[ApiController]
[Route("api/yahoofinance")]
public class YahooFinanceController : ControllerBase
{
    private readonly ILogger<YahooFinanceController> _logger;
    private readonly IYahooFinanceService _service;
    public YahooFinanceController(ILogger<YahooFinanceController> logger, IYahooFinanceService service)
    {
        _logger = logger;
        _service = service;
    }

    // Gets the intraday information based off of String symbol, validates the symbol
    // and then returns a list of daily values from the past month
    [HttpGet("intraday/{symbol}")]
    public async Task<ActionResult<IEnumerable<Intraday>>> GetIntradayList(String symbol)
    {
        IEnumerable<Intraday> intradayList = new List<Intraday>();
    
        // Classic 400 response because the user put a bad value
        // Not doing any symbol check here, only whitespace
        // Allowing yahoo finance api to handle invalid symbols
        if(string.IsNullOrWhiteSpace(symbol))
        {
            return BadRequest("Symbol is required");
        }
    
        try
        {
            intradayList = await _service.GetIntradayList(symbol);
        } 
        catch (HttpRequestException httpEx)
        {
            // Because we have a problem with the yahoo api it is 502 bad gateway
            _logger.LogError(httpEx, "HTTP request error while getting intraday list for symbol: {Symbol}", symbol);
            return StatusCode(502, "Invalid symbol or Yahoo API error");
        }
        catch (Exception ex)
        {
            // By default, anything unexpected is internal server
            _logger.LogError(ex, "Unexpected error while getting intraday list for symbol: {Symbol}", symbol);
            return StatusCode(500, "Internal server error");
        }
        
        // 404 Scenario if symbol exists, yahoo searches and returns success
        // but no intraday data found
        // Possible for stock exchanges like NASDAQ
        if (!intradayList.Any())
        {
            _logger.LogError("No intraday data found for symbol: {Symbol}", symbol);
            return NotFound("No intraday data found for this symbol: " + symbol);
        }
        
        return Ok(intradayList);
    }
}
