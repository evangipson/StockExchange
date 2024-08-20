using Microsoft.AspNetCore.Mvc;

using StockExchange.Domain.Models;
using StockExchange.Logic.Repositories.Interfaces;

namespace StockExchange.WebApi.Controllers
{
	/// <summary>
	/// The controller for the api which will get and set
	/// all <see cref="Company"/> data.
	/// </summary>
	[ApiController]
	[Route("/api/company")]
	public class CompanyController : Controller
	{
		private readonly ILogger<CompanyController> _logger;
		private readonly ICompanyRepository _companyRepository;

		public CompanyController(ICompanyRepository companyRepository, ILogger<CompanyController> logger)
		{
			_companyRepository = companyRepository;
			_logger = logger;
		}

		/// <summary>
		/// Gets any <see cref="Company"/>, or a list of every
		/// <see cref="Company"/>.
		/// </summary>
		/// <param name="tickerName"></param>
		/// <returns>
		/// A collection of <see cref="Company"/> when successful,
		/// <see cref="StatusCodes.Status500InternalServerError"/>
		/// otherwise.
		/// </returns>
		[HttpGet(Name = "Company")]
		public IActionResult GetAllCompanyData(string? tickerName)
		{
			var matchingCompanies = _companyRepository.GetEntity(tickerName);

			return matchingCompanies == null ? StatusCode(500) : Ok(matchingCompanies);
		}

		/// <summary>
		/// Creates a new <see cref="Company"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="Company"/> that was created,
		/// <see cref="StatusCodes.Status500InternalServerError"/>
		/// otherwise.
		/// </returns>
		[HttpPost(Name = "Company")]
		public IActionResult CreateCompany(Company? company)
		{
			if (company == null)
			{
				_logger.LogError($"{nameof(CreateCompany)}: Could not create company.");
				return StatusCode(500);
			}

			_companyRepository.SetEntity(company);
			_logger.LogInformation($"{nameof(CreateCompany)}: Created and saved company.");
			return Ok(company);
		}
	}
}
