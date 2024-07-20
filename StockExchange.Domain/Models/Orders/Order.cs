using StockExchange.Domain.Models.Actors;

namespace StockExchange.Domain.Models.Orders
{
	public class Order
	{
		public int OrderId { get; set; }

		public Actor? Seller { get; set; }

		public Actor? Buyer { get; set; }

		public Stock? Stock { get; set; }

		public int Amount { get; set; }

		public OrderType Type { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime DateFulfilled { get; set; }

		public bool Fulfilled { get; set; }
	}
}
