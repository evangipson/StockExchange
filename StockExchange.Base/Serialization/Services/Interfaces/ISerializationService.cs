using StockExchange.Base.Serialization.Models;

namespace StockExchange.Base.Serialization.Services.Interfaces
{
	/// <summary>
	/// A service which interfaces directly with the serializer and
	/// deserializer to get or set anything of the provided
	/// <typeparamref name="EntityType"/>.
	/// </summary>
	/// <typeparam name="EntityType">
	/// The type of item to be serialized and deserialized. Must inherit
	/// from <see cref="ISerializedEntity"/>.
	/// </typeparam>
	public interface ISerializationService<EntityType> where EntityType : ISerializedEntity, new()
	{
		/// <summary>
		/// Gets every <typeparamref name="EntityType"/> from the
		/// serialization source by means of deserialization.
		/// </summary>
		/// <param name="serializationFilePath">
		/// An optional parameter which contains a path to the
		/// file where the serialized entities are.
		/// </param>
		/// <returns>
		/// A collection of <typeparamref name="EntityType"/>.
		/// Defaults to an empty collection.
		/// </returns>
		IEnumerable<EntityType>? GetAll(string? serializationFilePath);

		/// <summary>
		/// Gets a single <typeparamref name="EntityType"/> from the
		/// serialization source. Uses <see cref="GetAll(string?)"/>
		/// to ensure the entities are in-memory before doing anything.
		/// </summary>
		/// <typeparam name="SerializedAccessor">
		/// The type which will be used to filter entity results.
		/// </typeparam>
		/// <param name="accessor">
		/// The value which will be used to filter entity results.
		/// </param>
		/// <param name="serializationFilePath">
		/// An optional parameter which contains a path to the
		/// file where the serialized entities are.
		/// </param>
		/// <returns>
		/// A single <typeparamref name="EntityType"/> if successful,
		/// <c>null</c> otherwise.
		/// </returns>
		EntityType? Get<SerializedAccessor>(SerializedAccessor accessor, string? serializationFilePath);

		/// <summary>
		/// Sets a single <typeparamref name="EntityType"/> in the
		/// serialization source by means of serialization.
		/// </summary>
		/// <param name="entity">
		/// The entity to set in the serialization source.
		/// </param>
		/// <param name="serializationFilePath">
		/// An optional parameter which contains a path to the
		/// file where the serialized entities are.
		/// </param>
		/// <returns>
		/// <c>true</c> when successful, <c>false</c> otherwise.
		/// </returns>
		bool Set(EntityType entity, string? serializationFilePath);
	}
}
