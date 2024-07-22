using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Serialization.Models;
using StockExchange.Base.Serialization.Services.Interfaces;

namespace StockExchange.Base.Serialization.Services
{
	[Service(typeof(ISerializationService<>))]
	public class SerializationService<EntityType> : ISerializationService<EntityType> where EntityType : ISerializedEntity, new()
	{
		private readonly ILogger<SerializationService<EntityType>> _logger;
		private readonly XmlSerializer _serializer;
		private readonly string _typeName;

		private SerializedList<EntityType> _entityList;

		public SerializationService(ILogger<SerializationService<EntityType>> logger)
		{
			_logger = logger;
			_serializer = new XmlSerializer(typeof(SerializedList<EntityType>));
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

			using (StreamReader streamReader = new(serializationFilePath))
			{
				if (streamReader == null)
				{
					_logger.LogError($"{nameof(Get)}: Couldn't open file for reading, did not get the {_typeName.ToLower()}.");
					return null;
				}

				using (StringReader stringReader = new(streamReader.ReadToEnd()))
				{
					_entityList = _serializer.Deserialize(stringReader) as SerializedList<EntityType>? ?? new SerializedList<EntityType>();
					_logger.LogInformation($"{nameof(Get)}: Got every {_typeName.ToLower()} from file.");
				}
			}

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

			// Ensure _entityList is in-memory via stored XML before setting anything
			List<EntityType>? entities = GetAll(serializationFilePath)?.ToList() ?? [];

			try
			{
				_logger.LogInformation($"{nameof(Set)}: Trying to update {_typeName.ToLower()}.");
				var xmlSettings = new XmlWriterSettings { Indent = true, NewLineOnAttributes = true, OmitXmlDeclaration = true, WriteEndDocumentOnClose = true };
				var xmlNamespace = new XmlSerializerNamespaces();
				xmlNamespace.Add("", "");

				UpdateInMemoryEntityList(entity);

				using (XmlWriter writer = XmlWriter.Create(serializationFilePath, xmlSettings))
				{
					if (writer == null)
					{
						_logger.LogError($"{nameof(Get)}: Couldn't open file for reading, did not get the {_typeName.ToLower()}.");
						return false;
					}

					_serializer.Serialize(writer, _entityList, xmlNamespace);
					_logger.LogInformation($"{nameof(Set)}: Updated {_typeName.ToLower()}.");
				}

				return true;
			}
			catch
			{
				_logger.LogError($"{nameof(Set)}: Couldn't update {_typeName.ToLower()}.");
				return false;
			}
		}

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
	}
}
