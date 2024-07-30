﻿using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Domain.Models.Orders;
using StockExchange.Domain.Models.Actors;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.Logic.Services
{
	[Service(typeof(IBrokerService))]
	public class BrokerService : IBrokerService
	{
		private readonly int buyingThreshold = 10;

		public bool ShouldBuy(Order order)
		{
			var brokerFromOrder = GetBrokerFromOrder(order);
			if (brokerFromOrder == null || brokerFromOrder != order.Buyer)
			{
				return false;
			}

			var brokerTendency = GetBrokerTransactionTendency(brokerFromOrder);
			var upside = order.Stock?.Close - order.Stock?.Price;

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
			var upside = order.Stock?.Price - order.Stock?.Close;

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
				buyingChance += 10;
			}
			if (broker.Behaviors.Contains(BrokerBehavior.Safe))
			{
				buyingChance -= 5;
			}

			return buyingChance;
		}
	}
}
