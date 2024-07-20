using StockExchange.Domain.Models;

namespace StockExchange.Logic.Services.Interfaces
{
	public interface IPricingService
	{
		decimal GetPrice(Stock stock);
	}
}
