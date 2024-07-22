﻿using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Resources.Services.Interfaces;
using StockExchange.Base.Serialization.Repositories;
using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Base.Serialization.Services.Interfaces;
using StockExchange.Domain.Models.Orders;

namespace StockExchange.Logic.Repositories
{
	[Service(typeof(ISerializableRepository<Order>))]
	public class OrderRepository : SerializableRepository<Order>
	{
		private readonly IResourceService _resourceService;

		public OrderRepository(ILogger<OrderRepository> logger, ISerializationService<Order> serializationService, IResourceService resourceService)
			: base(logger, serializationService)
		{
			_resourceService = resourceService;
		}

		protected override string? DatasourcePath => _resourceService.GetResourceFilePath("Orders.xml");

		public override IEnumerable<Order>? FilterEntities(string? filter)
		{
			return EntityList?.Where(order => order.OrderId.ToString() == filter);
		}
	}
}
