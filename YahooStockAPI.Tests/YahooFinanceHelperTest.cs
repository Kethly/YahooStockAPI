namespace YahooStockAPI.Tests;
using FluentAssertions;
using YahooStockAPI.Api.Helpers;

public class YahooFinanceHelperTests
{
    // Test input: unix time
    // Expected output: proper date and formatted string
    [Fact]
    public void TestDateConversion()
    {

        var result = YahooFinanceHelper.ConvertToDateTime(1765981800);
        result.Should().Be(new DateTime(2025, 12, 17, 14, 30, 0));

        var dateString = YahooFinanceHelper.ConvertDateTimeToString(new DateTime(2025, 9, 17));
        dateString.Should().Be("2025-09-17");
    }

    // Test input: long decimal value
    // Expected output: rounded to four positions.
    [Fact]
    public void TestRounding()
    {
        var helper = new YahooFinanceHelper();
        var result = YahooFinanceHelper.RoundToFourDecimalPlaces(123.456789);
        result.Should().Be(123.4568);
    }

    // Test input: list of doubles
    // Expected output: average of the list
    [Fact]
    public void TestAverageCalculation()
    {
        var values = new List<double> { 1.0, 2.0, 3.0, 4.0 };
        var result = YahooFinanceHelper.CalculateAverage(values);
        result.Should().Be(2.5);
    }
}