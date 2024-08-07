using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Domain.Models.Orders;
using StockExchange.Domain.Models.Events;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.Logic.Services
{
	[Service(typeof(IOrderService))]
	public class OrderService : IOrderService
	{
		private readonly ILogger<OrderService> _logger;
		private readonly ISerializableRepository<Order> _orderRepository;
		private readonly IBrokerService _brokerService;

		private readonly List<IObserver<Order>> _observers;

		public OrderService(ILogger<OrderService> logger, ISerializableRepository<Order> orderRepository, IBrokerService brokerService)
		{
			_logger = logger;
			_orderRepository = orderRepository;
			_brokerService = brokerService;
			_observers = [];

			Subscribe(brokerService);
		}

		public virtual IDisposable Subscribe(IObserver<Order> observer)
		{
			if (!_observers.Contains(observer))
			{
				_observers.Add(observer);
			}

			return new IUnsubscriber<Order>.Unsubscriber(_observers, observer);
		}

		public IEnumerable<Order>? GetOrderBook() => _orderRepository.GetEntity();

		public bool PlaceOrder(Order order)
		{
			_logger.LogInformation($"{nameof(PlaceOrder)}: Trying to add order {order.OrderId} to the order book.");
			if(order.Buyer == null || order.Seller == null)
			{
				_logger.LogError($"{nameof(PlaceOrder)}: Order {order.OrderId} had no buyer or seller, returning false.");
				return false;
			}

			var brokerBuyingChance = _brokerService.ShouldBuy(order);
			var brokerSellingChance = _brokerService.ShouldSell(order);
			_logger.LogInformation($"{nameof(PlaceOrder)}: broker would buy - {brokerBuyingChance}, broker would sell - {brokerSellingChance}");

			_orderRepository.SetEntity(order);

			_logger.LogInformation($"{nameof(PlaceOrder)}: Notifying all observers that an order has been placed.");
			_observers.ForEach(observer => observer.OnNext(order));

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
