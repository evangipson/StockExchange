namespace StockExchange.Base.Serialization.Interfaces
{
    public interface ISerializationService<EntityType> where EntityType : class, new()
	{
		IEnumerable<EntityType> GetAll(string xmlFilePath);

		EntityType Get<SerializedAccessor>(SerializedAccessor accessor, string xmlFilePath);

		bool Set(EntityType entity, string xmlFilePath);
	}
}
