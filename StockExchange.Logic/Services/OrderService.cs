using Microsoft.Extensions.Logging;
using StockExchange.Domain.Models.Orders;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.Logic.Services
{
	public class OrderService : IOrderService
	{
		private readonly ILogger<OrderService> _logger;
		private Queue<Order>? _orderBook;

		public OrderService(ILogger<OrderService> logger)
		{
			_logger = logger;
		}

		public Queue<Order> OrderBook
		{
			get => _orderBook ??= new Queue<Order>();
		}

		public bool PlaceOrder(Order order)
		{
			_logger.LogInformation($"{nameof(PlaceOrder)}: Placing order.");
			return false;
		}
	}
}
