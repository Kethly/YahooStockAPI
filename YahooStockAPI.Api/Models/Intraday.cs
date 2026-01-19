namespace YahooStockAPI.Models;

public class Intraday
{
    // the date in format YYYY-MM-DD
    public DateTime Day { get; set; }
    // the average of the lows in the day up to 4 decimal points 
    public double LowAverage { get; set; }
    // the average of the highs in the day up to 4 decimal points
    public double HighAverage { get; set; }
    // the cumulative volume in a day
    public long Volume { get; set; }
}