using Microsoft.AspNetCore.Mvc;

using StockExchange.Domain.Models.Orders;
using StockExchange.Logic.Factories.Interfaces;
using StockExchange.Logic.Repositories.Interfaces;
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
		private readonly IBrokerRepository _brokerRepository;
		private readonly ICompanyRepository _companyRepository;

		public OrderController(
			ILogger<OrderController> logger,
			IOrderFactory orderFactory,
			IOrderService orderService,
            IBrokerRepository brokerRepository,
            ICompanyRepository companyRepository
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
			var company = _companyRepository.GetEntity("Michaelsoft")?.FirstOrDefault();
			var order = _orderFactory.CreateOrder(buyer, seller, company, 10.00m);
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
