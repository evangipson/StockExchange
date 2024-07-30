using StockExchange.Base.Serialization.Models;

namespace StockExchange.Domain.Models
{
	public class Cryptocurrency : ISerializedEntity
	{
		public string? Name { get; set; }

		public int Amount { get; set; }

		public string? ShortName { get; set; }

		public decimal Value { get; set; }

		public DateTime CreatedDate { get; set; }

		// ISerializedEntity properties
		public string ElementName => "Cryptocurrency";

		public string FileName => "crypto.save";

		public Guid EntityId { get; set; }
	}
}
