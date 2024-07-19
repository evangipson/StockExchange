using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection;
using StockExchange.Base.Serialization.Interfaces;
using StockExchange.Domain.Models;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.Logic.Services
{
	[Service(typeof(ICompanyService))]
	public class CompanyService : ICompanyService
	{
		private readonly ILogger<CompanyService> _logger;
		private readonly ISerializationService<Company> _serializationService;
		private const string _dataPath = "C:\\projects\\StockExchange\\StockExchange.Domain\\Data\\Companies.xml";

		public CompanyService(ILogger<CompanyService> logger, ISerializationService<Company> serializationService)
		{
			_logger = logger;
			_serializationService = serializationService;
		}

		public IEnumerable<Company>? GetCompany(string? companyName = null)
		{
			var companyList = _serializationService.GetAll(_dataPath);

			if (string.IsNullOrEmpty(companyName))
			{
				return companyList;
			}

			var companies = companyList.Where(company =>
			{
				return company.TickerName == companyName || company.Name == companyName;
			});

			if (companies.Count() == 0)
			{
				_logger.LogError($"{nameof(GetCompany)}: No company with the name {companyName} exists.");
				return null;
			}

			return companies;
		}

		public bool SetCompany(Company company) => _serializationService.Set(company, _dataPath);
	}
}
