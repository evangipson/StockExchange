using StockExchange.Domain.Models.Orders;

namespace StockExchange.Logic.Factories.Interfaces
{
	public interface IOrderFactory
	{
		Order? CreateOrder();
	}
}
