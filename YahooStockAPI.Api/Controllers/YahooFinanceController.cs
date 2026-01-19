using Microsoft.AspNetCore.Mvc;
using YahooStockAPI.Models;

namespace YahooStockAPI.Api.Controllers;

[ApiController]
[Route("api/yahoofinance")]
public class YahooFinanceController : ControllerBase
{
    private readonly ILogger<YahooFinanceController> _logger;

    public YahooFinanceController(ILogger<YahooFinanceController> logger)
    {
        _logger = logger;
    }

    // Gets the intraday information based off of String symbol, validates the symbol
    // and then returns a list of daily values from the past month
    // TODO: create a service and helper that returns the list of intradays
    [HttpGet("intraday/{symbol}")]
    public async Task<ActionResult<IEnumerable<Intraday>>> GetIntradayList(String symbol)
    {
        IEnumerable<Intraday> intradayList = new List<Intraday>();
        if(string.IsNullOrWhiteSpace(symbol))
        {
            return BadRequest("Symbol is required");
        }
        return Ok(intradayList);
    }
}
