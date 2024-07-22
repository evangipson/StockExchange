using Microsoft.AspNetCore.Mvc;

using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Domain.Models;

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
		private readonly ISerializableRepository<Company> _companyRepository;

		public CompanyController(ISerializableRepository<Company> companyRepository)
		{
			_companyRepository = companyRepository;
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
	}
}
