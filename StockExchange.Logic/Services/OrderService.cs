using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Domain.Models.Orders;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.Logic.Services
{
	[Service(typeof(IOrderService))]
	public class OrderService : IOrderService
	{
		private readonly ILogger<OrderService> _logger;
		private readonly ISerializableRepository<Order> _orderRepository;

		public OrderService(ILogger<OrderService> logger, ISerializableRepository<Order> orderRepository)
		{
			_logger = logger;
			_orderRepository = orderRepository;
		}

		public IEnumerable<Order>? GetOrderBook() => _orderRepository.GetEntity();

		public bool PlaceOrder(Order order)
		{
			_logger.LogInformation($"{nameof(PlaceOrder)}: Adding order {order.OrderId} to the order book.");
			_orderRepository.SetEntity(order);

			return true;
		}

		public bool FulfillOrder(Order order)
		{
			order.Fulfilled = true;

			_logger.LogInformation($"{nameof(FulfillOrder)}: Fulfilling order {order.OrderId} from the order book.");
			_orderRepository.SetEntity(order);
			return true;
		}
	}
}
