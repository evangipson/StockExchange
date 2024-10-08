﻿using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Serialization.Services.Interfaces;
using StockExchange.Base.Serialization.Repositories;
using StockExchange.Base.Resources.Services.Interfaces;
using StockExchange.Domain.Models;
using StockExchange.Logic.Repositories.Interfaces;

namespace StockExchange.Logic.Repositories
{
	/// <summary>
	/// An implementation of <see cref="SerializableRepository{SerializedType}"/>,
	/// which will serialize and deserialize a <see cref="Company"/>.
	/// </summary>
	[Service(typeof(ICompanyRepository))]
	public class CompanyRepository : SerializableRepository<Company>, ICompanyRepository
	{
		private readonly IResourceService _resourceService;

		public CompanyRepository(ILogger<CompanyRepository> logger, ISerializationService<Company> serializationService, IResourceService resourceService)
			: base(logger, serializationService)
		{
			_resourceService = resourceService;
		}

		protected override string? DatasourcePath => _resourceService.GetResourceFilePath("companies.save");

		public override IEnumerable<Company>? FilterEntities(string? filter)
		{
			return EntityList?.Where(entity => entity.Name == filter || entity.TickerName == filter);
		}
	}
}
