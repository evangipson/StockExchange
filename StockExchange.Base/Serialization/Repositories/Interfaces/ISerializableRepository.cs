using StockExchange.Base.Serialization.Models;

namespace StockExchange.Base.Serialization.Repositories.Interfaces
{
	public interface ISerializableRepository<SerializedType> where SerializedType : ISerializedEntity, new()
	{
		abstract IEnumerable<SerializedType>? GetEntity(string? entityIdentifier = null);

		abstract bool SetEntity(SerializedType entity);
	}
}
