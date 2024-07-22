using StockExchange.Base.Serialization.Models;
using StockExchange.Base.Serialization.Services.Interfaces;

namespace StockExchange.Base.Serialization.Repositories.Interfaces
{
	/// <summary>
	/// An abstract service that handles getting and setting any
	/// <typeparamref name="SerializedType"/> that is provided.
	/// Leverages <see cref="ISerializationService{EntityType}"/>,
	/// but doesn't contain any implementation around the actual
	/// serialization strategy.
	/// <para>
	/// Intended to be used as a base for any service that will
	/// get or set serialization data for any <see cref="Type"/>.
	/// </para>
	/// </summary>
	/// <typeparam name="SerializedType">
	/// The type that will be serialized and deserialized.
	/// Must inherit from <see cref="ISerializedEntity"/>.
	/// </typeparam>
	public interface ISerializableRepository<SerializedType> where SerializedType : ISerializedEntity, new()
	{
		/// <summary>
		/// Gets an entity or entities from serialization. If no
		/// <paramref name="entityIdentifier"/> is provided, the
		/// entire collection of entities will be returned.
		/// </summary>
		/// <param name="entityIdentifier">
		/// An optional filtering parameter that will restrict
		/// results when provided.
		/// </param>
		/// <returns>
		/// A list of entities. Defaults to an empty collection
		/// of <typeparamref name="SerializedType"/>.
		/// </returns>
		IEnumerable<SerializedType>? GetEntity(string? entityIdentifier = null);

		/// <summary>
		/// Sets an entity in serialization. Will update the in-memory
		/// list of entities before setting anything.
		/// </summary>
		/// <param name="entity">
		/// The entity to update in serialization.
		/// </param>
		/// <returns>
		/// <c>true</c> when serialization is successful, <c>false</c>
		/// otherwise.
		/// </returns>
		bool SetEntity(SerializedType entity);

		/// <summary>
		/// Filters entity results. Intended to be overridden by
		/// implementations.
		/// <para>
		/// Used in <see cref="GetEntity(string?)"/> to filter
		/// entity results.
		/// </para>
		/// </summary>
		/// <param name="filter">
		/// An optional value, used to filter entity results when
		/// it is provided.
		/// </param>
		/// <returns>
		/// A collection of entities. Defaults to an empty collection
		/// of <typeparamref name="SerializedType"/>.
		/// </returns>
		IEnumerable<SerializedType>? FilterEntities(string? filter);
	}
}
