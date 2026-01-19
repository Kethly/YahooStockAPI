// The tests use the real Yahoo Finance API, but they can be converted to mock data
namespace YahooStockAPI.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using YahooStockAPI.Api.Controllers;
using YahooStockAPI.Api.Services;
using YahooStockAPI.Models;

public class YahooFinanceControllerTests
{
    // Test input: empty or whitespace request
    // Expected output: bad request status
    [Fact]
    public async Task TestGetSymbolEmpty()
    {
        var logger = new Mock<ILogger<YahooFinanceController>>();
        var serviceLogger = new Mock<ILogger<YahooFinanceService>>();
        var controller = new YahooFinanceController(logger.Object, new YahooFinanceService(new HttpClient(), serviceLogger.Object));

        var result = await controller.GetIntradayList("   ");
        result.Result.Should().BeOfType<BadRequestObjectResult>();

        result = await controller.GetIntradayList("");
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    // Test input: tesla ticker
    // Expected output: 200 or ok response
    [Fact]
    public async Task TestGetSymbolSuccessful()
    {
        var logger = new Mock<ILogger<YahooFinanceController>>();
        var serviceLogger = new Mock<ILogger<YahooFinanceService>>();
        var controller = new YahooFinanceController(logger.Object, new YahooFinanceService(new HttpClient(), serviceLogger.Object));

        var result = await controller.GetIntradayList("TSLA");

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    // Test input: NASDAQ ticker
    // Expected output: 404 or not found response
    [Fact]
    public async Task TestSearchStockExchange()
    {
        var logger = new Mock<ILogger<YahooFinanceController>>();
        var serviceLogger = new Mock<ILogger<YahooFinanceService>>();
        var controller = new YahooFinanceController(logger.Object, new YahooFinanceService(new HttpClient(), serviceLogger.Object));

        var result = await controller.GetIntradayList("NASDAQ");

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    // Test input: symbol ticker that is searched but doesn't exist
    // Expected output: 502 or internal server error response
    [Fact]
    public async Task TestInvalidSymbol()
    {
        var logger = new Mock<ILogger<YahooFinanceController>>();
        var serviceLogger = new Mock<ILogger<YahooFinanceService>>();
        var controller = new YahooFinanceController(logger.Object, new YahooFinanceService(new HttpClient(), serviceLogger.Object));

        var result = await controller.GetIntradayList("invalidsymbol123");

        result.Result.Should().BeOfType<ObjectResult>().Which.StatusCode.Should().Be(502);
    }

}
