using StockExchange.Base.Serialization.Models;

namespace StockExchange.Base.Serialization.Repositories.Interfaces
{
	public interface ISerializableRepository<SerializedType> where SerializedType : ISerializedEntity, new()
	{
		IEnumerable<SerializedType>? GetEntity(string? entityIdentifier = null);

		bool SetEntity(SerializedType entity);

		IEnumerable<SerializedType>? FilterEntities(string? filter);
	}
}
