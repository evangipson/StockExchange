using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Domain.Models;
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

		public Order? CreateOrder(Actor? buyer, Actor? seller, Company? company, decimal ask)
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
			if (company != null)
			{
				newOrder.Company = company;
			}
			newOrder.Ask = ask;

			return newOrder;
		}
	}
}
