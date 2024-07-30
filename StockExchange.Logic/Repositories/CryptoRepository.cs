using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Resources.Services.Interfaces;
using StockExchange.Base.Serialization.Repositories;
using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Base.Serialization.Services.Interfaces;
using StockExchange.Domain.Models;

namespace StockExchange.Logic.Repositories
{
	/// <summary>
	/// An implementation of <see cref="SerializableRepository{SerializedType}"/>,
	/// which will serialize and deserialize a <see cref="Cryptocurrency"/>.
	/// </summary>
	[Service(typeof(ISerializableRepository<Cryptocurrency>))]
	public class CryptoRepository : SerializableRepository<Cryptocurrency>
	{
		private readonly IResourceService _resourceService;

		public CryptoRepository(ILogger<CryptoRepository> logger, ISerializationService<Cryptocurrency> serializationService, IResourceService resourceService)
			: base(logger, serializationService)
		{
			_resourceService = resourceService;
		}

		protected override string? DatasourcePath => _resourceService.GetResourceFilePath("crypto.save");

		public override IEnumerable<Cryptocurrency>? FilterEntities(string? filter)
		{
			return EntityList?.Where(order => order.Name?.ToString() == filter || order.ShortName?.ToString() == filter);
		}
	}
}
