using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Domain.Models.Orders;
using StockExchange.Domain.Models.Actors;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.Logic.Services
{
	[Service(typeof(IBrokerService))]
	public class BrokerService : IBrokerService
	{
		private readonly ILogger<BrokerService> _logger;
		private readonly int buyingThreshold = 10;
		private readonly Random _random = new();

		private IDisposable? _unsubscriber;
		private bool first = false;
		private Order? last;

		public BrokerService(ILogger<BrokerService> logger)
		{
			_logger = logger;
		}

		public virtual void Subscribe(IObservable<Order> provider)
		{
			_unsubscriber = provider.Subscribe(this);
		}

		public virtual void Unsubscribe()
		{
			_unsubscriber?.Dispose();
		}

		public virtual void OnCompleted()
		{
			_logger.LogInformation($"{nameof(OnCompleted)}: Additional order data will not be transmitted.");
		}

		public virtual void OnError(Exception error)
		{
			// Do nothing.
		}

		public virtual void OnNext(Order order)
		{
			_logger.LogInformation($"{nameof(OnNext)}: Order being evaluated by broker.");
			// base the amount of time they wait off of the broker's behavior
			Thread.Sleep(_random.Next(500, 2500));

			if (first)
			{
				last = order;
				first = false;
				return;
			}

			_logger.LogInformation($"{nameof(OnNext)}: Order update being evaluated by broker.");
		}

		public bool ShouldBuy(Order order)
		{
			var brokerFromOrder = GetBrokerFromOrder(order);
			if (brokerFromOrder == null || brokerFromOrder != order.Buyer)
			{
				return false;
			}

			var brokerTendency = GetBrokerTransactionTendency(brokerFromOrder);
			var upside = order.Company?.LatestStockPrice - order.Ask;

			_logger.LogInformation($"{nameof(ShouldBuy)}: broker tendency for buy threshold = {brokerTendency}");
			_logger.LogInformation($"{nameof(ShouldBuy)}: upside for buy threshold = {upside}");

			return brokerTendency + upside > buyingThreshold;
		}

		public bool ShouldSell(Order order)
		{
			var brokerFromOrder = GetBrokerFromOrder(order);
			if (brokerFromOrder == null || brokerFromOrder != order.Seller)
			{
				return false;
			}

			var brokerTendency = GetBrokerTransactionTendency(brokerFromOrder);
            var upside = order.Ask - order.Company?.LatestStockPrice;

			_logger.LogInformation($"{nameof(ShouldSell)}: broker tendency for buy threshold = {brokerTendency}");
			_logger.LogInformation($"{nameof(ShouldSell)}: upside for buy threshold = {upside}");

			return brokerTendency + upside > buyingThreshold;
		}

		private Broker? GetBrokerFromOrder(Order order)
		{
			if(order.Buyer?.GetType().Name == "Broker")
			{
				return order.Buyer as Broker;
			}
			if(order.Seller?.GetType().Name == "Broker")
			{
				return order.Seller as Broker;
			}

			return null;
		}

		private int GetBrokerTransactionTendency(Broker broker)
		{
			var buyingChance = 0;
			if (broker.Behaviors.Contains(BrokerBehavior.Aggressive))
			{
				buyingChance += buyingThreshold;
			}
			if (broker.Behaviors.Contains(BrokerBehavior.Safe))
			{
				buyingChance -= buyingThreshold / 2;
			}

			return buyingChance;
		}
	}
}
