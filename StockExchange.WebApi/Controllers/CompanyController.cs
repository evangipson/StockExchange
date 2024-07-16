using Microsoft.AspNetCore.Mvc;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.WebApi.Controllers
{
	[ApiController]
	[Route("/api/company")]
	public class CompanyController : Controller
	{
		private readonly ISerializationService _serializationService;

		public CompanyController(ISerializationService serializationService)
		{
			_serializationService = serializationService;
		}
	
		[HttpGet(Name = "Company")]
		public IActionResult GetAllCompanyData(string? tickerName)
		{
			var matchingCompany = _serializationService.GetCompany(tickerName);
			return matchingCompany.Count() > 0 ? Ok(matchingCompany) : NotFound();
		}
	}
}
