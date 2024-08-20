using Microsoft.AspNetCore.Mvc;

using StockExchange.Domain.Models;
using StockExchange.Logic.Repositories.Interfaces;

namespace StockExchange.WebApi.Controllers
{
	/// <summary>
	/// The controller for the api which will get and set
	/// all <see cref="Cryptocurrency"/> data.
	/// </summary>
	[ApiController]
	[Route("/api/crypto")]
	public class CryptoController : Controller
	{
		private readonly ILogger<CryptoController> _logger;
		private readonly ICryptoRepository _cryptoRepository;

		public CryptoController(ICryptoRepository cryptoRepository, ILogger<CryptoController> logger)
		{
			_cryptoRepository = cryptoRepository;
			_logger = logger;
		}

		/// <summary>
		/// Gets any <see cref="Cryptocurrency"/>, or a list of every
		/// <see cref="Cryptocurrency"/>.
		/// </summary>
		/// <param name="cryptoName"></param>
		/// <returns>
		/// A collection of <see cref="Cryptocurrency"/> when successful,
		/// <see cref="StatusCodes.Status500InternalServerError"/>
		/// otherwise.
		/// </returns>
		[HttpGet(Name = "Crypto")]
		public IActionResult GetAllCryptocurrencyData(string? cryptoName)
		{
			var matchingCryptocurrencies = _cryptoRepository.GetEntity(cryptoName);

			return matchingCryptocurrencies == null ? StatusCode(500) : Ok(matchingCryptocurrencies);
		}

		/// <summary>
		/// Creates a new <see cref="Cryptocurrency"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="Company"/> that was created,
		/// <see cref="StatusCodes.Status500InternalServerError"/>
		/// otherwise.
		/// </returns>
		[HttpPost(Name = "Crypto")]
		public IActionResult CreateCryptocurrency(Cryptocurrency? cryptocurrency)
		{
			if (cryptocurrency == null)
			{
				_logger.LogError($"{nameof(CreateCryptocurrency)}: Could not create cryptocurrency.");
				return StatusCode(500);
			}

			_cryptoRepository.SetEntity(cryptocurrency);
			_logger.LogInformation($"{nameof(CreateCryptocurrency)}: Created and saved cryptocurrency.");
			return Ok(cryptocurrency);
		}
	}
}
