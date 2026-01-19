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
    public async Task getSymbolEmpty()
    {
        var logger = new Mock<ILogger<YahooFinanceController>>();
        var controller = new YahooFinanceController(logger.Object, Mock.Of<YahooFinanceService>());

        var result = await controller.GetIntradayList("   ");
        result.Result.Should().BeOfType<BadRequestObjectResult>();

        result = await controller.GetIntradayList("");
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    // Test input: tesla sticker
    // Expected output: 200 or ok response
    [Fact]
    public async Task getSymbolSuccessful()
    {
        var logger = new Mock<ILogger<YahooFinanceController>>();
        var controller = new YahooFinanceController(logger.Object, Mock.Of<YahooFinanceService>());

        var result = await controller.GetIntradayList("TSLA");

        result.Result.Should().BeOfType<OkObjectResult>();
    }
}