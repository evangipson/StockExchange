using Microsoft.AspNetCore.Mvc;

using StockExchange.Base.Serialization.Repositories.Interfaces;
using StockExchange.Domain.Models;
using StockExchange.Domain.Models.Actors;
using StockExchange.Domain.Models.Orders;
using StockExchange.Logic.Factories.Interfaces;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.WebApi.Controllers
{
	/// <summary>
	/// The controller for the api which will get and set
	/// all <see cref="Order"/> data.
	/// </summary>
	[ApiController]
	[Route("/api/order")]
	public class OrderController : Controller
	{
		private readonly ILogger<OrderController> _logger;
		private readonly IOrderFactory _orderFactory;
		private readonly IOrderService _orderService;
		private readonly ISerializableRepository<Broker> _brokerRepository;
		private readonly ISerializableRepository<Company> _companyRepository;

		public OrderController(
			ILogger<OrderController> logger,
			IOrderFactory orderFactory,
			IOrderService orderService,
			ISerializableRepository<Broker> brokerRepository,
			ISerializableRepository<Company> companyRepository
		){
			_logger = logger;
			_orderFactory = orderFactory;
			_orderService = orderService;
			_brokerRepository = brokerRepository;
			_companyRepository = companyRepository;
		}

		/// <summary>
		/// Gets every <see cref="Order"/>.
		/// </summary>
		/// <returns>
		/// A collection of every <see cref="Order"/>.
		/// </returns>
		[HttpGet(Name = "Order")]
		public IActionResult GetOrderBook() => Ok(_orderService.GetOrderBook());

		/// <summary>
		/// Creates a new <see cref="Order"/>, and places
		/// that <see cref="Order"/>.
		/// </summary>
		/// <returns>
		/// The <see cref="Order"/> that was created and placed,
		/// <see cref="StatusCodes.Status500InternalServerError"/>
		/// otherwise.
		/// </returns>
		[HttpPost(Name = "Order")]
		public IActionResult CreateOrder()
		{
			var buyer = _brokerRepository.GetEntity("I-Trade")?.FirstOrDefault();
			var seller = _brokerRepository.GetEntity("Findelity")?.FirstOrDefault();
			var stock = _companyRepository.GetEntity("Michaelsoft")?.FirstOrDefault()?.Stock;
			var order = _orderFactory.CreateOrder(buyer, seller, stock);
			if (order == null)
			{
				_logger.LogError($"{nameof(CreateOrder)}: Could not create order.");
				return StatusCode(500);
			}

			var orderPlaced = _orderService.PlaceOrder(order);
			return orderPlaced ? Ok(order) : StatusCode(500);
		}
	}
}
