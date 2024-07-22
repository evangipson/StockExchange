using Microsoft.AspNetCore.Mvc;

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

		public OrderController(ILogger<OrderController> logger, IOrderFactory orderFactory, IOrderService orderService)
		{
			_logger = logger;
			_orderFactory = orderFactory;
			_orderService = orderService;
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
			var order = _orderFactory.CreateOrder();
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
