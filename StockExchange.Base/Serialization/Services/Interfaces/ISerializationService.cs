using StockExchange.Base.Serialization.Models;

namespace StockExchange.Base.Serialization.Services.Interfaces
{
	public interface ISerializationService<EntityType> where EntityType : ISerializedEntity, new()
	{
		IEnumerable<EntityType>? GetAll(string? serializationFilePath);

		EntityType? Get<SerializedAccessor>(SerializedAccessor accessor, string? serializationFilePath);

		bool Set(EntityType entity, string? serializationFilePath);
	}
}
