namespace YahooStockAPI.Models;

public class Intraday
{
    // the date in format YYYY-MM-DD
    public DateTime day { get; set; }
    // the average of the lows in the day up to 4 decimal points 
    public double lowAverage { get; set; }
    // the average of the highs in the day up to 4 decimal points
    public double highAverage { get; set; }
    // the cumulative volume in a day
    public int volume { get; set; }
}