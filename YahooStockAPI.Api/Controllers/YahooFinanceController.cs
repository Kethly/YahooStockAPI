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
    // TODO: create a service and helper that returns the list of intradays
    [HttpGet("intraday/{symbol}")]
    public async Task<ActionResult<IEnumerable<Intraday>>> GetIntradayList(String symbol)
    {
        IEnumerable<Intraday> intradayList = new List<Intraday>();
        if(string.IsNullOrWhiteSpace(symbol))
        {
            return BadRequest("Symbol is required");
        }
        intradayList = await _service.GetIntradayList(symbol);
        return Ok(intradayList);
    }
}
