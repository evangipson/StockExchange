using System.Xml.Serialization;

namespace StockExchange.Base.Serialization
{
	[XmlRoot("SerializedList")]
	public struct SerializedList<EntityType>() where EntityType : class, new()
	{
		private List<EntityType> _entities;

		[XmlElement("SerializedItem")]
		public List<EntityType> Entities
		{
			get => _entities ??= [];
		}
	}
}
