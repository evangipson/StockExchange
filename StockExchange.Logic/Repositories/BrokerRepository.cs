using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Serialization.Repositories;
using StockExchange.Base.Serialization.Services.Interfaces;
using StockExchange.Base.Resources.Services.Interfaces;
using StockExchange.Domain.Models.Actors;
using StockExchange.Logic.Repositories.Interfaces;

namespace StockExchange.Logic.Repositories
{
	/// <summary>
	/// An implementation of <see cref="SerializableRepository{SerializedType}"/>,
	/// which will serialize and deserialize a <see cref="Broker"/>.
	/// </summary>
	[Service(typeof(IBrokerRepository))]
	public class BrokerRepository : SerializableRepository<Broker>, IBrokerRepository
	{
		private readonly IResourceService _resourceService;

		public BrokerRepository(ILogger<BrokerRepository> logger, ISerializationService<Broker> serializationService, IResourceService resourceService)
			: base(logger, serializationService)
		{
			_resourceService = resourceService;
		}

		protected override string? DatasourcePath => _resourceService.GetResourceFilePath("brokers.save");

		public override IEnumerable<Broker>? FilterEntities(string? filter)
		{
			return EntityList?.Where(entity => entity.BrokerageName == filter);
		}
	}
}
