using StockExchange.Domain.Models;

namespace StockExchange.Logic.Services.Interfaces
{
	public interface IStockExchangeService
	{
		decimal GetStockPrice(Stock stock);

		bool PlaceOrder(Order order);
	}
}
