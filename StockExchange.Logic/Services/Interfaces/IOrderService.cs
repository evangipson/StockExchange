using StockExchange.Domain.Models.Orders;

namespace StockExchange.Logic.Services.Interfaces
{
	public interface IOrderService
	{
		IEnumerable<Order>? GetOrderBook();

		bool PlaceOrder(Order order);

		bool FulfillOrder(Order order);
	}
}
