using StockExchange.Domain.Models.Orders;

namespace StockExchange.Logic.Services.Interfaces
{
	public interface IBrokerService
	{
		bool ShouldBuy(Order order);

		bool ShouldSell(Order order);
	}
}
