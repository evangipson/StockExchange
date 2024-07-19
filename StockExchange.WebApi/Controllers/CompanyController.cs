using Microsoft.AspNetCore.Mvc;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.WebApi.Controllers
{
	[ApiController]
	[Route("/api/company")]
	public class CompanyController : Controller
	{
		private readonly ICompanyService _companyService;

		public CompanyController(ICompanyService companyService)
		{
			_companyService = companyService;
		}
	
		[HttpGet(Name = "Company")]
		public IActionResult GetAllCompanyData(string? tickerName)
		{
			var matchingCompanies = _companyService.GetCompany(tickerName);

			return matchingCompanies == null ? NotFound() : Ok(matchingCompanies);
		}
	}
}
