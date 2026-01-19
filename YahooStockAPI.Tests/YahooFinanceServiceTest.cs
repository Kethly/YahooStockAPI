namespace YahooStockAPI.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using YahooStockAPI.Api.Services;
using YahooStockAPI.Models;

public class YahooFinanceServiceTests
{
    // Test input: empty or whitespace request
    // Expected output: bad request status
    [Fact]
    public async Task TestGetServiceSymbolEmpty()
    {
        var logger = new Mock<ILogger<YahooFinanceService>>();
        var service = new YahooFinanceService(new HttpClient(), logger.Object);

        Func<Task> act = async () => await service.GetIntradayList("   ");

        await act.Should().ThrowAsync<Exception>()
        .WithMessage("Symbol is required");
        
        act = async () => await service.GetIntradayList("");

        await act.Should().ThrowAsync<Exception>()
        .WithMessage("Symbol is required");
    }

    // Test input: invalid symbol
    // Expected output: exception thrown
    [Fact]
    public async Task TestGetServiceSymbolInvalid()
    {
        var logger = new Mock<ILogger<YahooFinanceService>>();
        var service = new YahooFinanceService(new HttpClient(), logger.Object);
        Func<Task> act = async () => await service.GetIntradayList("INVALIDSYMBOL123");
        await act.Should().ThrowAsync<Exception>()
        .WithMessage("Failed to retrieve data for symbol: INVALIDSYMBOL123");
    }

    // Test input: tesla sticker
    // Expected output: 200 or ok response
    [Fact]
    public async Task TestGetServiceSymbolSuccessful()
    {
        var logger = new Mock<ILogger<YahooFinanceService>>();
        var service = new YahooFinanceService(new HttpClient(), logger.Object);

        var result = await service.GetIntradayList("TSLA");

        result.Should().NotBeNull();
        result.Should().BeOfType<List<Intraday>>();
    }
}