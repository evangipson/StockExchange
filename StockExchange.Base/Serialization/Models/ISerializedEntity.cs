namespace StockExchange.Base.Serialization.Models
{
	public interface ISerializedEntity
	{
		string ElementName { get; }

		string FileName { get; }

		Guid EntityId { get; }
	}
}
