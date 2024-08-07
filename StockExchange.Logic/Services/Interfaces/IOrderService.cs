using StockExchange.Domain.Models.Events;
using StockExchange.Domain.Models.Orders;

namespace StockExchange.Logic.Services.Interfaces
{
	public interface IOrderService : IUnsubscriber<Order>, IObservable<Order>
    {
		IEnumerable<Order>? GetOrderBook();

		bool PlaceOrder(Order order);

		bool FulfillOrder(Order order);
	}
}
