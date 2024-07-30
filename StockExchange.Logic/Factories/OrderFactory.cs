using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Domain.Models.Actors;
using StockExchange.Domain.Models.Orders;
using StockExchange.Logic.Factories.Interfaces;

namespace StockExchange.Logic.Factories
{
	[Service(typeof(IOrderFactory), Lifetime = ServiceLifetime.Transient)]
	public class OrderFactory : IOrderFactory
	{
		private readonly ILogger<OrderFactory> _logger;

		public OrderFactory(ILogger<OrderFactory> logger)
		{
			_logger = logger;
		}

		public Order? CreateOrder(Actor? buyer, Actor? seller)
		{
			_logger.LogInformation($"{nameof(CreateOrder)}: Creating order.");

			var newOrder = new Order
			{
				OrderId = Guid.NewGuid(),
				DateCreated = DateTime.UtcNow
			};

			if(buyer != null)
			{
				newOrder.Buyer = buyer;
			}
			if (seller != null)
			{
				newOrder.Seller = seller;
			}

			return newOrder;
		}
	}
}
