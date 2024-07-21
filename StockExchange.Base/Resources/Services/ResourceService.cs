using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Resources.Services.Interfaces;

namespace StockExchange.Base.Resources.Services
{
	[Service(typeof(IResourceService))]
	public class ResourceService : IResourceService
	{
		private readonly string _projectPath = Path.GetFullPath("../StockExchange.Base/Resources/Data");

		public string? GetResourceFilePath(string resourceName)
		{
			return Path.GetFullPath(resourceName, _projectPath);
		}
	}
}
