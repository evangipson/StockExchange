using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Domain.Models;
using StockExchange.Domain.Models.Orders;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.Logic.Services
{
	[Service(typeof(IStockExchangeService))]
	public class StockExchangeService : IStockExchangeService
	{
		private readonly IPricingService _pricingService;

		public StockExchangeService(IPricingService pricingService)
		{
			_pricingService = pricingService;
		}

		public decimal GetStockPrice(Stock stock)
		{
			return _pricingService.GetPrice(stock);
		}

		public bool PlaceOrder(Order order)
		{
			return false;
		}
	}
}
