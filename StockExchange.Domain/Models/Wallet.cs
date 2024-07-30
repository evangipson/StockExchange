namespace StockExchange.Domain.Models
{
	public class Wallet
	{
		/// <summary>
		/// United States dollar.
		/// </summary>
		public decimal USD { get; set; }

		/// <summary>
		/// Canadian dollar.
		/// </summary>
		public decimal CAD { get; set; }

		/// <summary>
		/// British pound.
		/// </summary>
		public decimal GBP { get; set; }

		/// <summary>
		/// Australian dollar.
		/// </summary>
		public decimal AUD { get; set; }

		/// <summary>
		/// Euros.
		/// </summary>
		public decimal EUR { get; set; }

		/// <summary>
		/// Mexican peso.
		/// </summary>
		public decimal MXN { get; set; }

		/// <summary>
		/// Indian rupee.
		/// </summary>
		public decimal INR { get; set; }

		/// <summary>
		/// Swedish krona.
		/// </summary>
		public decimal SEK { get; set; }

		/// <summary>
		/// Norweigian krone.
		/// </summary>
		public decimal NOK { get; set; }

		/// <summary>
		/// Japanese yen.
		/// </summary>
		public int JPY { get; set; }

		/// <summary>
		/// Chinese yuan.
		/// </summary>
		public decimal RMB { get; set; }

		/// <summary>
		/// Hong Kong dollar.
		/// </summary>
		public decimal HKD { get; set; }

		/// <summary>
		/// Singapore dollar.
		/// </summary>
		public decimal SGD { get; set; }

		/// <summary>
		/// Russian ruble.
		/// </summary>
		public int RUB { get; set; }

		/// <summary>
		/// Total accumulated value of all
		/// currencies.
		/// </summary>
		public decimal TotalValue { get; set; }

		/// <summary>
		/// A list of all cryptocurrencies associated
		/// with this wallet.
		/// </summary>
		public List<Cryptocurrency> Cryptocurrencies { get; set; } = [];
	}
}
