using Microsoft.AspNetCore.Mvc;
using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Domain.Models;
using StockExchange.Domain.Models.Actors;

namespace StockExchange.WebApi.Controllers
{
	/// <summary>
	/// The controller for the api which will get and set
	/// all <see cref="Broker"/> data.
	/// </summary>
	[ApiController]
	[Route("/api/broker")]
	public class BrokerController : Controller
	{
		private readonly ILogger<BrokerController> _logger;
		private readonly ISerializableRepository<Broker> _brokerRepository;

		public BrokerController(ISerializableRepository<Broker> brokerRepository, ILogger<BrokerController> logger)
		{
			_brokerRepository = brokerRepository;
			_logger = logger;
		}

		/// <summary>
		/// Gets any <see cref="Broker"/>, or a list of every
		/// <see cref="Broker"/>.
		/// </summary>
		/// <param name="brokerName"></param>
		/// <returns>
		/// A collection of <see cref="Broker"/> when successful,
		/// <see cref="StatusCodes.Status500InternalServerError"/>
		/// otherwise.
		/// </returns>
		[HttpGet(Name = "Broker")]
		public IActionResult GetAllBrokerData(string? brokerName)
		{
			var matchingCryptocurrencies = _brokerRepository.GetEntity(brokerName);

			return matchingCryptocurrencies == null ? StatusCode(500) : Ok(matchingCryptocurrencies);
		}

		/// <summary>
		/// Creates a new <see cref="Broker"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="Broker"/> that was created,
		/// <see cref="StatusCodes.Status500InternalServerError"/>
		/// otherwise.
		/// </returns>
		[HttpPost(Name = "Broker")]
		public IActionResult CreateBroker(Broker? broker)
		{
			if (broker == null)
			{
				_logger.LogError($"{nameof(CreateBroker)}: Could not create broker.");
				return StatusCode(500);
			}

			_brokerRepository.SetEntity(broker);
			_logger.LogInformation($"{nameof(CreateBroker)}: Created and saved broker.");
			return Ok(broker);
		}
	}
}
