namespace YahooStockAPI.Api.Helpers;

public class YahooFinanceHelper
{
    // Helper methods for Yahoo Finance API can be added here in the future
    public DateTime convertToDateTime(long unixTime)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
        return dateTimeOffset.DateTime;
    }

    // Converts the DateTime to appropriate format YYYY-MM-DD
    public String convertDateTimeToString(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }

    // Rounds a double to four decimal places, for stock high and lows output
    public double roundToFourDecimalPlaces(double value)
    {
        return Math.Round(value, 4);
    }

    // Calculates average of a list of doubles
    public double calculateAverage(List<double> values)
    {
        if (values == null || values.Count == 0)
        {
            return 0;
        }
        return values.Average();
    }
}