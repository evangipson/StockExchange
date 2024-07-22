namespace StockExchange.Base.Resources.Services.Interfaces
{
	/// <summary>
	/// A service that handles getting information related
	/// to resources for the project.
	/// </summary>
	public interface IResourceService
	{
		/// <summary>
		/// Gets a <see cref="Path"/> for a resource contained
		/// in the project.
		/// </summary>
		/// <param name="resourceName">
		/// The name of the resource file to get.
		/// </param>
		/// <returns>
		/// A <see cref="Path"/> for the requested resource.
		/// </returns>
		string GetResourceFilePath(string resourceName);
	}
}
