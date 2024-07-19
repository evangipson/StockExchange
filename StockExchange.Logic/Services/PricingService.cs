using StockExchange.Base.DependencyInjection;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.Logic.Services
{
	[Service(typeof(IPricingService))]
	public class PricingService : IPricingService
	{
	}
}
