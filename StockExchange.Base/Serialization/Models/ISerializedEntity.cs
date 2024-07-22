namespace StockExchange.Base.Serialization.Models
{
	/// <summary>
	/// A contract for an entity which will be serialized
	/// and deserialized.
	/// </summary>
	public interface ISerializedEntity
	{
		/// <summary>
		/// The name of the element, usually the name of the type
		/// which implements <see cref="ISerializedEntity"/>.
		/// </summary>
		string ElementName { get; }

		/// <summary>
		/// The name of the file which will contain the serialization
		/// data for this entity.
		/// </summary>
		string FileName { get; }

		/// <summary>
		/// The id of this entity.
		/// </summary>
		Guid EntityId { get; }
	}
}
