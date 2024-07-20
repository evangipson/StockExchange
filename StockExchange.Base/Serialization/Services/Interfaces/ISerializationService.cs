namespace StockExchange.Base.Serialization.Services.Interfaces
{
	public interface ISerializationService<EntityType> where EntityType : class, new()
	{
		IEnumerable<EntityType>? GetAll(Stream? serializationSource);

		EntityType? Get<SerializedAccessor>(SerializedAccessor accessor, Stream? serializationSource);

		bool Set(EntityType entity, Stream? serializationSource);
	}
}
