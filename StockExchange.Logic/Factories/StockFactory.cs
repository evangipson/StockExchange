using StockExchange.Domain.Models;
using StockExchange.Logic.Factories.Interfaces;

namespace StockExchange.Logic.Factories
{
	public class StockFactory : IStockFactory
	{
		public Stock MakeStock()
		{
			return new Stock();
		}
	}
}
