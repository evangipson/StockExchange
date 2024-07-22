using System.Xml.Serialization;

namespace StockExchange.Base.Serialization.Models
{
	/// <summary>
	/// Represents a collection of items that will be serialized
	/// and deserialized.
	/// </summary>
	/// <typeparam name="EntityType">
	/// The type of entity to serialize/deserialize.
	/// </typeparam>
	/// <param name="entities">
	/// An optional parameter that will be used to fill up
	/// <see cref="Entities"/>, if it is provided.
	/// </param>
	[XmlRoot("SerializedList")]
	public struct SerializedList<EntityType>(List<EntityType>? entities) where EntityType : ISerializedEntity, new()
	{
		/// <summary>
		/// Represents the collection of items that will be serialized
		/// and deserialized as XML nodes. Defaults to an empty list
		/// of <typeparamref name="EntityType"/>.
		/// </summary>
		[XmlElement("SerializedItem")]
		public List<EntityType> Entities { readonly get; set; } = entities ??= [];
	}
}
