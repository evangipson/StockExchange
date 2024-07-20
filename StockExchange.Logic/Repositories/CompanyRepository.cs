using System.Reflection;
using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Serialization.Services.Interfaces;
using StockExchange.Base.Resources.Services.Interfaces;
using StockExchange.Domain.Models;
using StockExchange.Logic.Repositories.Interfaces;

namespace StockExchange.Logic.Repositories
{
	[Service(typeof(ICompanyRepository))]
	public class CompanyRepository : ICompanyRepository
	{
		private readonly ILogger<CompanyRepository> _logger;
		private readonly ISerializationService<Company> _serializationService;
		private readonly IResourceService _resourceService;
		private readonly Stream? _companyDatasource;

		public CompanyRepository(ILogger<CompanyRepository> logger, ISerializationService<Company> serializationService, IResourceService resourceService)
		{
			_logger = logger;
			_serializationService = serializationService;
			_resourceService = resourceService;
			_companyDatasource = _resourceService.GetEmbeddedResourceStream("Companies.xml");
		}

		public IEnumerable<Company>? GetCompany(string? companyName = null)
		{
			var companyList = _serializationService.GetAll(_companyDatasource);

			if (string.IsNullOrEmpty(companyName))
			{
				return companyList;
			}

			var companies = companyList?.Where(company =>
			{
				return company.TickerName == companyName || company.Name == companyName;
			});

			if (companies?.Count() == 0)
			{
				_logger.LogError($"{nameof(GetCompany)}: No company with the name {companyName} exists.");
				return null;
			}

			return companies;
		}

		public bool UpdateCompany(Company company)
		{
			if (_companyDatasource == null)
			{
				_logger.LogError($"{nameof(GetCompany)}: Company data not found. Can't update company.");
				return false;
			}

			return _serializationService.Set(company, _companyDatasource);
		}
	}
}
