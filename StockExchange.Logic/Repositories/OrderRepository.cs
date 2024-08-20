using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Resources.Services.Interfaces;
using StockExchange.Base.Serialization.Repositories;
using StockExchange.Base.Serialization.Services.Interfaces;
using StockExchange.Domain.Models.Orders;
using StockExchange.Logic.Repositories.Interfaces;

namespace StockExchange.Logic.Repositories
{
	/// <summary>
	/// An implementation of <see cref="SerializableRepository{SerializedType}"/>,
	/// which will serialize and deserialize an <see cref="Order"/>.
	/// </summary>
	[Service(typeof(IOrderRepository))]
	public class OrderRepository : SerializableRepository<Order>, IOrderRepository
	{
		private readonly IResourceService _resourceService;

		public OrderRepository(ILogger<OrderRepository> logger, ISerializationService<Order> serializationService, IResourceService resourceService)
			: base(logger, serializationService)
		{
			_resourceService = resourceService;
		}

		protected override string? DatasourcePath => _resourceService.GetResourceFilePath("orders.save");

		public override IEnumerable<Order>? FilterEntities(string? filter)
		{
			return EntityList?.Where(order => order.OrderId.ToString() == filter);
		}
	}
}
