using StockExchange.Domain.Models.Orders;

namespace StockExchange.Logic.Services.Interfaces
{
	public interface IBrokerService : IObserver<Order>
    {
		bool ShouldBuy(Order order);

		bool ShouldSell(Order order);
	}
}
