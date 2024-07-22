using System.Globalization;

namespace StockExchange.Domain.Models
{
	/// <summary>
	/// Represents a <see cref="Company"/> stock.
	/// </summary>
	public class Stock
	{
		public decimal Price { get; set; }

		public decimal Open { get; set; }

		public decimal Close { get; set; }

		public decimal High { get; set; }

		public decimal Low { get; set; }

		public decimal MarketCap { get; set; }

		public decimal Volume { get; set; }

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
