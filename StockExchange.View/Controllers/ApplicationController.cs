using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection;
using StockExchange.Domain.Models;
using StockExchange.View.Controllers.Interfaces;

namespace StockExchange.View.Controllers
{
	/// <inheritdoc cref="IApplicationController" />
	[Service(typeof(IApplicationController))]
	public class ApplicationController : IApplicationController
	{
		private readonly ILogger<ApplicationController> _logger;

		public ApplicationController(ILogger<ApplicationController> logger)
		{
			_logger = logger;
		}

		public void Run()
		{
			// Run the application logic here
			_logger.LogInformation("Started the application.");
			LoadData();
		}

		private void LoadData()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CompanyList));
			CompanyList companyList;
			using (StreamReader reader = new StreamReader("C:\\projects\\StockExchange\\StockExchange.Domain\\Data\\Companies.xml"))
			{
				companyList = serializer.Deserialize(reader) as CompanyList ?? new CompanyList();
			}

			companyList?.Companies?.ForEach(company =>
			{
				_logger.LogInformation(company?.ToString());
			});
		}
	}
}
