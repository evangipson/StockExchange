using System.Reflection;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Resources.Services.Interfaces;

namespace StockExchange.Base.Resources.Services
{
	[Service(typeof(IResourceService))]
	public class ResourceService : IResourceService
	{
		public Stream? GetEmbeddedResourceStream(string resourceName)
		{
			var embeddedResourceName = GetEmbeddedResourceFilepath(resourceName);
			var embeddedResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName);
			return embeddedResourceStream;
		}

		private string GetEmbeddedResourceFilepath(string resourceName)
		{
			return GetEmbeddedResourceNames().First(file => file.Contains(resourceName));
		}

		/// <summary>
		/// Get the list of all emdedded resources in the assembly.
		/// </summary>
		/// <returns>An array of fully qualified resource names</returns>
		private string[] GetEmbeddedResourceNames()
		{
			return Assembly.GetExecutingAssembly().GetManifestResourceNames();
		}
	}
}
