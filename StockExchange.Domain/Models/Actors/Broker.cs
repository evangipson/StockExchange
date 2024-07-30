using StockExchange.Base.Serialization.Models;

namespace StockExchange.Domain.Models.Actors
{
	public enum BrokerBehavior
	{
		Aggressive,
		Passive,
		Safe,
		Trending,
		CuttingEdge
	}

	public class Broker : Actor, ISerializedEntity
	{
		public string? BrokerageName { get; set; }

		public List<BrokerBehavior> Behaviors { get; set; } = [];

		// ISerializedEntity properties
		public string ElementName => "Broker";

		public string FileName => "brokers.save";

		public Guid EntityId { get; set; }
	}
}
