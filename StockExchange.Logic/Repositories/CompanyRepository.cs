using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Serialization.Services.Interfaces;
using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Base.Serialization.Repositories;
using StockExchange.Base.Resources.Services.Interfaces;
using StockExchange.Domain.Models;

namespace StockExchange.Logic.Repositories
{
	[Service(typeof(ISerializableRepository<Company>))]
	public class CompanyRepository : SerializableRepository<Company>
	{
		private readonly IResourceService _resourceService;

		public CompanyRepository(ILogger<CompanyRepository> logger, ISerializationService<Company> serializationService, IResourceService resourceService)
			: base(logger, serializationService)
		{
			_resourceService = resourceService;
		}

		protected override string? DatasourcePath => _resourceService.GetResourceFilePath("Companies.xml");

		public override IEnumerable<Company>? FilterEntities(string? filter)
		{
			return EntityList?.Where(entity => entity.Name == filter || entity.TickerName == filter);
		}
	}
}
