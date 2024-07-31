using StockExchange.Domain.Models;
using StockExchange.Domain.Models.Actors;
using StockExchange.Domain.Models.Orders;

namespace StockExchange.Logic.Factories.Interfaces
{
	public interface IOrderFactory
	{
		Order? CreateOrder(Actor? buyer, Actor? seller, Stock? stock);
	}
}
