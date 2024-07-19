namespace StockExchange.Domain.Models
{
	public class Order
	{
		public Stock? Stock { get; set; }

		public decimal Amount { get; set; }

		public DateTime Date { get; set; }

		public OrderType Type { get; set; }
	}
}
