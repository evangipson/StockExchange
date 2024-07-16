using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection;
using StockExchange.View.Controllers.Interfaces;

namespace StockExchange.View
{
	internal class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// <param name="args">
		/// A list of arguments provided to the application.
		/// </param>
		private static void Main(string[] args)
		{
			Console.OutputEncoding = System.Text.Encoding.UTF8;
			Console.Title = "Stock Exchange";

			// setup our DI
			var serviceCollection = new ServiceCollection().AddLogging(cfg => cfg.AddConsole());

			// add StockExchange services from StockExchange.Services and StockExchange.View using reflection
			serviceCollection.AddServicesFromAssembly(Assembly.GetAssembly(typeof(IApplicationController)));

			// instantiate depenedency injection concrete object
			var serviceProvider = serviceCollection.BuildServiceProvider();

			// start the application by getting the Application
			// class from the required services, and run it.
			serviceProvider.GetRequiredService<IApplicationController>().Run();

			return;
		}
	}
}