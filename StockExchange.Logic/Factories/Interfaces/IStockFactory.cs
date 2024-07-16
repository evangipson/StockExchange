using StockExchange.Domain.Models;

namespace StockExchange.Logic.Factories.Interfaces
{
	public interface IStockFactory
	{
		Stock MakeStock();
	}
}
