using StockExchange.Domain.Models.Orders;

namespace StockExchange.Domain.Models.Actors
{
    public abstract class Actor
	{
		public int Id { get; set; }

		public Order? LastPurchase { get; set; }

		public Wallet? Wallet { get; set; }
	}
}
