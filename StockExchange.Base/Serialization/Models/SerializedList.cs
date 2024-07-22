using System.Xml.Serialization;

namespace StockExchange.Base.Serialization.Models
{
	[XmlRoot("SerializedList")]
	public struct SerializedList<EntityType>(List<EntityType>? entities) where EntityType : ISerializedEntity, new()
	{
		[XmlElement("SerializedItem")]
		public List<EntityType> Entities { readonly get; set; } = entities ??= [];
	}
}
