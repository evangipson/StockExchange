namespace StockExchange.Base.Resources.Services.Interfaces
{
	public interface IResourceService
	{
		/// <summary>
		/// Gets an embedded resource as a <see cref="Stream"/>.
		/// </summary>
		/// <param name="resourceName">
		/// The file name of the embedded resource.
		/// </param>
		/// <returns>
		/// A <see cref="Stream"/> of an embedded resource,
		/// defaults to <c>null</c>.
		/// </returns>
		Stream? GetEmbeddedResourceStream(string resourcePath);
	}
}
