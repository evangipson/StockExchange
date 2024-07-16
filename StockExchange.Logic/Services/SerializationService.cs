using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection;
using StockExchange.Domain.Models;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.Logic.Services
{
	[Service(typeof(ISerializationService))]
	public class SerializationService : ISerializationService
	{
		private readonly ILogger<SerializationService> _logger;
		private readonly XmlSerializer _serializer;
		private CompanyList _companyList = new CompanyList();

		public SerializationService(ILogger<SerializationService> logger)
        {
			_logger = logger;
			_serializer = new XmlSerializer(typeof(CompanyList));
		}

        public IEnumerable<Company> GetCompany(string? companyName = null)
		{
			_logger.LogInformation($"{nameof(GetCompany)}: Getting {companyName ?? "all companies"}.");
			using (StreamReader reader = new StreamReader("C:\\projects\\StockExchange\\StockExchange.Domain\\Data\\Companies.xml"))
			{
				_companyList = _serializer.Deserialize(reader) as CompanyList ?? new CompanyList();
			}

			if(string.IsNullOrEmpty(companyName))
			{
				return _companyList.Companies;
			}

			var companyToUpdate = _companyList.Companies.FirstOrDefault(company => company.TickerName == companyName)
				?? _companyList.Companies.FirstOrDefault(company => company.Name == companyName);
			
			if(companyToUpdate == null)
			{
				_logger.LogError($"{nameof(GetCompany)}: No company with the name {companyName} exists.");
				return new List<Company> { new Company() };
			}
			return new List<Company> { companyToUpdate };
		}

		public bool SetCompany(Company company)
		{
			try
			{
				_logger.LogInformation($"{nameof(GetCompany)}: Trying to update {company.Name}.");
				using (StreamWriter writer = new StreamWriter("C:\\projects\\StockExchange\\StockExchange.Domain\\Data\\Companies.xml"))
				{
					_serializer.Serialize(writer, company);
					_logger.LogInformation($"{nameof(GetCompany)}: Updated {company.Name}.");
					return true;
				}
			}
			catch
			{
				_logger.LogError($"{nameof(GetCompany)}: Couldn't update {company.Name}.");
				return false;
			}
		}
	}
}
