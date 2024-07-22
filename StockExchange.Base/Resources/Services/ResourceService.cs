using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Resources.Services.Interfaces;

namespace StockExchange.Base.Resources.Services
{
	/// <inheritdoc cref="IResourceService"/>
	[Service(typeof(IResourceService))]
	public class ResourceService : IResourceService
	{
		/// <summary>
		/// The base directory where resources are in
		/// the project.
		/// </summary>
		private readonly string _projectPath = Path.GetFullPath("../StockExchange.Base/Resources/Data");

		public string GetResourceFilePath(string resourceName)
		{
			return Path.GetFullPath(resourceName, _projectPath);
		}
	}
}
