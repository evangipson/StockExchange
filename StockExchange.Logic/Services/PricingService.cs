using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Domain.Models;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.Logic.Services
{
	[Service(typeof(IPricingService))]
	public class PricingService : IPricingService
	{
		public decimal GetPrice(Stock stock)
		{
			return stock.Price * GetDemandScore(stock);
		}

		private decimal GetDemandScore(Stock stock)
		{
			return 1m;
		}
	}
}
