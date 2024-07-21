using Microsoft.AspNetCore.Mvc;

using StockExchange.Logic.Factories.Interfaces;
using StockExchange.Logic.Services.Interfaces;

namespace StockExchange.WebApi.Controllers
{
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

		[HttpGet(Name = "Order")]
		public IActionResult GetOrderBook() => Ok(_orderService.GetOrderBook());

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
