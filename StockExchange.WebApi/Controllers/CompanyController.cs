using Microsoft.AspNetCore.Mvc;
using StockExchange.Logic.Repositories.Interfaces;

namespace StockExchange.WebApi.Controllers
{
	[ApiController]
	[Route("/api/company")]
	public class CompanyController : Controller
	{
		private readonly ICompanyRepository _companyRepository;

		public CompanyController(ICompanyRepository companyRepository)
		{
			_companyRepository = companyRepository;
		}

		[HttpGet(Name = "Company")]
		public IActionResult GetAllCompanyData(string? tickerName)
		{
			var matchingCompanies = _companyRepository.GetCompany(tickerName);

			return matchingCompanies == null ? NotFound() : Ok(matchingCompanies);
		}
	}
}
