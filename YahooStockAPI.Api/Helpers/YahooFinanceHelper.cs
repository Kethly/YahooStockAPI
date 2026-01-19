/* YahooFinanceHelper
   Provides reusable, static utility functions for date conversion, rounding,
   and statistical calculations used by the service layer. Intended to
   to be scalable for other endpoints and classes */
namespace YahooStockAPI.Api.Helpers;

public class YahooFinanceHelper
{
    // Helper methods for Yahoo Finance API can be added here in the future
    public static DateTime ConvertToDateTime(long unixTime)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTime);
        return dateTimeOffset.DateTime;
    }

    // Converts the DateTime to appropriate format YYYY-MM-DD
    public static String ConvertDateTimeToString(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd");
    }

    // Rounds a double to four decimal places, for stock high and lows output
    public static double RoundToFourDecimalPlaces(double value)
    {
        return Math.Round(value, 4);
    }

    // Calculates average of a list of doubles
    public static double CalculateAverage(List<double> values)
    {
        if (values == null || values.Count == 0)
        {
            return 0;
        }
        return values.Average();
    }
}