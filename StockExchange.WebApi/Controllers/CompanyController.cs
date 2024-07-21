using Microsoft.AspNetCore.Mvc;

using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Domain.Models;

namespace StockExchange.WebApi.Controllers
{
	[ApiController]
	[Route("/api/company")]
	public class CompanyController : Controller
	{
		private readonly ISerializableRepository<Company> _companyRepository;

		public CompanyController(ISerializableRepository<Company> companyRepository)
		{
			_companyRepository = companyRepository;
		}

		[HttpGet(Name = "Company")]
		public IActionResult GetAllCompanyData(string? tickerName)
		{
			var matchingCompanies = _companyRepository.GetEntity(tickerName);

			return matchingCompanies == null ? StatusCode(500) : Ok(matchingCompanies);
		}
	}
}
