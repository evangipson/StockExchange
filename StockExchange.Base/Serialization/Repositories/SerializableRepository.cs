﻿using Microsoft.Extensions.Logging;

using StockExchange.Base.Serialization.Models;
using StockExchange.Base.Serialization.Services.Interfaces;
using StockExchange.Base.Serialization.Repositories.Interfaces;

namespace StockExchange.Base.Serialization.Repositories
{
	/// <inheritdoc cref="ISerializableRepository{SerializedType}"/>
	public abstract class SerializableRepository<SerializedType> : ISerializableRepository<SerializedType> where SerializedType : ISerializedEntity, new()
	{
		private readonly ILogger<SerializableRepository<SerializedType>> _logger;
		private readonly ISerializationService<SerializedType> _serializationService;
		private readonly string _typeName = typeof(SerializedType).Name;

		protected SerializableRepository(ILogger<SerializableRepository<SerializedType>> logger, ISerializationService<SerializedType> serializationService)
		{
			_logger = logger;
			_serializationService = serializationService;
		}

		/// <summary>
		/// A path to the datasource which is used in serialization. Intended to
		/// be set by the consumer of <see cref="SerializableRepository{SerializedType}"/>.
		/// </summary>
		protected abstract string? DatasourcePath { get; }

		/// <summary>
		/// A collection of <typeparamref name="SerializedType"/> which contains
		/// all the entities to serialize.
		/// </summary>
		protected IEnumerable<SerializedType>? EntityList { get; private set; }

		public virtual IEnumerable<SerializedType>? GetEntity(string? entityIdentifier = null)
		{
			if (string.IsNullOrEmpty(DatasourcePath))
			{
				_logger.LogError($"{nameof(SetEntity)}: {_typeName} data file path not found. Can't get {_typeName.ToLower()}.");
				return null;
			}

			EntityList = _serializationService.GetAll(DatasourcePath);
			if (string.IsNullOrEmpty(entityIdentifier))
			{
				return EntityList;
			}

			var filteredEntities = FilterEntities(entityIdentifier);
			if (filteredEntities?.Count() == 0)
			{
				_logger.LogError($"{nameof(GetEntity)}: No \"{entityIdentifier}\" {_typeName.ToLower()} exists in memory.");
				return null;
			}
			return filteredEntities;
		}

		public virtual bool SetEntity(SerializedType entity)
		{
			if (string.IsNullOrEmpty(DatasourcePath))
			{
				_logger.LogError($"{nameof(SetEntity)}: {_typeName} data file path not found. Can't set {_typeName.ToLower()}.");
				return false;
			}

			return _serializationService.Set(entity, DatasourcePath);
		}

		public abstract IEnumerable<SerializedType>? FilterEntities(string? filter);
	}
}
