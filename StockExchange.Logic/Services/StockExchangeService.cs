using StockExchange.Base.DependencyInjection;
using StockExchange.Domain.Models;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.Logic.Services
{
	[Service(typeof(IStockExchangeService))]
	public class StockExchangeService : IStockExchangeService
	{
		public decimal GetStockPrice(Stock stock)
		{
			return stock.Price;
		}

		public bool PlaceOrder(Order order)
		{
			return false;
		}
	}
}
