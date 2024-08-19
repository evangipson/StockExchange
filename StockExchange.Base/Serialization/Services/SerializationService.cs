using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Serialization.Models;
using StockExchange.Base.Serialization.Services.Interfaces;
using StockExchange.Base.Serialization.Extensions;

namespace StockExchange.Base.Serialization.Services
{
	/// <inheritdoc cref="ISerializationService{EntityType}"/>
	[Service(typeof(ISerializationService<>))]
	public class SerializationService<EntityType> : ISerializationService<EntityType> where EntityType : ISerializedEntity, new()
	{
		private readonly ILogger<SerializationService<EntityType>> _logger;
		private readonly string _typeName;

		private SerializedList<EntityType> _entityList;

		public SerializationService(ILogger<SerializationService<EntityType>> logger)
		{
			_logger = logger;
			_typeName = typeof(EntityType).Name;
			_entityList = new SerializedList<EntityType>([]);
		}

		public IEnumerable<EntityType>? GetAll(string? serializationFilePath)
		{
			if (string.IsNullOrEmpty(serializationFilePath))
			{
				_logger.LogInformation($"{nameof(Get)}: {_typeName} datasource file path not found. Can't get {_typeName.ToLower()}.");
				return null;
			}

			if (_entityList.Entities.Count != 0)
			{
				_logger.LogInformation($"{nameof(Get)}: Already had {_typeName.ToLower()} list in-memory.");
				return _entityList.Entities;
			}

			SerializedList<EntityType> entityList = SerializationExtensions.DeserializeStructFromFile<SerializedList<EntityType>>(serializationFilePath)
				?? new SerializedList<EntityType>([]);

			if (entityList.Entities.Count == 0)
			{
				_logger.LogWarning($"{nameof(Get)}: Couldn't deserialize contents of {serializationFilePath}, attempting to create new serialization file.");
				CreateSerializationFile(serializationFilePath);
			}
			else
			{
				_logger.LogInformation($"{nameof(Get)}: Got every {_typeName.ToLower()} from file.");
			}

			_entityList = entityList;
			return _entityList.Entities;
		}

		public EntityType? Get<SerializedAccessor>(SerializedAccessor accessor, string? serializationFilePath)
		{
			IEnumerable<EntityType>? entities = GetAll(serializationFilePath) ?? [];

			return entities.FirstOrDefault(entity =>
			{
				var entityProperties = typeof(EntityType).GetProperties();
				foreach (var property in entityProperties)
				{
					if (property.GetValue(entity)?.ToString() == accessor?.ToString())
					{
						_logger.LogInformation($"{nameof(Get)}: Got the \"{accessor}\" {_typeName.ToLower()}.");
						return true;
					}
				}
				_logger.LogWarning($"{nameof(Get)}: Could not get the \"{accessor}\" {_typeName.ToLower()}.");
				return false;
			});
		}

		public bool Set(EntityType entity, string? serializationFilePath)
		{
			if (string.IsNullOrEmpty(serializationFilePath))
			{
				_logger.LogInformation($"{nameof(Get)}: {_typeName} datasource file path not found. Can't set {_typeName.ToLower()}.");
				return false;
			}

			// Ensure _entityList is in-memory via deserialization before setting anything
			List<EntityType>? entities = GetAll(serializationFilePath)?.ToList() ?? [];

			try
			{
				_logger.LogInformation($"{nameof(Set)}: Trying to update {_typeName.ToLower()}.");
				UpdateInMemoryEntityList(entity);

				_entityList.SerializeToFile(serializationFilePath);
				_logger.LogInformation($"{nameof(Set)}: Updated {_typeName.ToLower()}.");

				return true;
			}
			catch
			{
				_logger.LogError($"{nameof(Set)}: Couldn't update {_typeName.ToLower()}.");
				return false;
			}
		}

		/// <summary>
		/// Updates the in-memory entity list using the provided
		/// <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">
		/// The entity to add to the in-memory entity list.
		/// </param>
		private void UpdateInMemoryEntityList(EntityType entity)
		{
			var matchedEntity = _entityList.Entities.Find(entityListEntry => entityListEntry.Equals(entity));
			if (matchedEntity == null)
			{
				_logger.LogInformation($"{nameof(Set)}: Couldn't find {_typeName.ToLower()} to update, creating a new entity in serialization.");
				_entityList.Entities.Add(entity);
			}
			else
			{
				_logger.LogInformation($"{nameof(Set)}: Found {_typeName.ToLower()} to update.");
				_entityList.Entities.Remove(matchedEntity);
				_entityList.Entities.Add(entity);
			}
		}

		/// <summary>
		/// Creates a serialization file if one does not exist.
		/// <para>
		/// Does nothing if <paramref name="serializationFilePath"/>
		/// is <c>null</c> or empty.
		/// </para>
		/// </summary>
		/// <param name="serializationFilePath">
		/// A required parameter which contains a path to the file
		/// that will be created.
		/// </param>
		private void CreateSerializationFile(string serializationFilePath)
		{
			if(!File.Exists(serializationFilePath))
			{
				File.Create(serializationFilePath);
				_logger.LogInformation($"{nameof(CreateSerializationFile)}: Created serialization file \"{serializationFilePath}\", because it could not be found.");
				return;
			}
			_logger.LogInformation($"{nameof(CreateSerializationFile)}: Serialization file \"{serializationFilePath}\", already exists, so it was not created.");
		}
	}
}
