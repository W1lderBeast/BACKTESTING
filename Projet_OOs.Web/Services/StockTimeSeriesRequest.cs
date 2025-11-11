using AlphaVantage.Net.Common.Intervals;

namespace Projet_OOS.Web.Services
{
    internal class StockTimeSeriesRequest
    {
        private string symbol;
        private Interval daily;

        public StockTimeSeriesRequest(string symbol, Interval daily)
        {
            this.symbol = symbol;
            this.daily = daily;
        }

        public object TimeSeriesSize { get; set; }
    }
}