using Microsoft.AspNetCore.Mvc;

using StockExchange.Domain.Models.Actors;
using StockExchange.Logic.Repositories.Interfaces;

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
		private readonly IBrokerRepository _brokerRepository;

		public BrokerController(IBrokerRepository brokerRepository, ILogger<BrokerController> logger)
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
			var matchingBrokers = _brokerRepository.GetEntity(brokerName);

			return matchingBrokers == null ? StatusCode(500) : Ok(matchingBrokers);
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
