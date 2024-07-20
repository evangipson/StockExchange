using StockExchange.Domain.Models.Orders;

namespace StockExchange.Logic.Services.Interfaces
{
	public interface IOrderService
	{
		bool PlaceOrder(Order order);
	}
}
