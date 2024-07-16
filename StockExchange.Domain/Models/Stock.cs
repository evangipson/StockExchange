using System.Globalization;

namespace StockExchange.Domain.Models
{
	public class Stock
	{
		public float Open { get; set; }

		public float Close { get; set; }

		public float High { get; set; }

		public float Low { get; set; }

		public float MarketCap { get; set; }

		public float Volume { get; set; }

		public override string ToString()
		{
			var stockData = new List<string>
			{
				$"\tOpen: {Open.ToString("C", CultureInfo.CurrentCulture)}",
				$"\tClose: {Close.ToString("C", CultureInfo.CurrentCulture)}",
				$"\tHigh: {High.ToString("C", CultureInfo.CurrentCulture)}",
				$"\tLow: {Low.ToString("C", CultureInfo.CurrentCulture)}",
				$"\tMarketCap: {MarketCap.ToString("C", CultureInfo.CurrentCulture)}",
				$"\tVolume: {Volume.ToString("C", CultureInfo.CurrentCulture)}",
			};
			return string.Join("\n\t", stockData);
		}
	}
}
