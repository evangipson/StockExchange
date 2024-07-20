using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using StockExchange.Base.DependencyInjection.Attributes;
using StockExchange.Base.Serialization.Models;
using StockExchange.Base.Serialization.Services.Interfaces;

namespace StockExchange.Base.Serialization.Services
{
	[Service(typeof(ISerializationService<>))]
	public class SerializationService<EntityType> : ISerializationService<EntityType> where EntityType : class, new()
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
		}

		public IEnumerable<EntityType>? GetAll(Stream? serializationSource)
		{
			if (serializationSource == null)
			{
				_logger.LogError($"{nameof(GetAll)}: {_typeName} data not found. Can't get {_typeName.ToLower()}.");
				return null;
			}

			if (_entityList.Entities.Count != 0)
			{
				_logger.LogInformation($"{nameof(Get)}: Already had {_typeName.ToLower()} list in-memory.");
				return _entityList.Entities;
			}

			using (StreamReader reader = new StreamReader(serializationSource))
			{
				_logger.LogInformation($"{nameof(Get)}: Got every {_typeName.ToLower()} from file.");
				_entityList = _serializer.Deserialize(reader) as SerializedList<EntityType>? ?? new SerializedList<EntityType>();
			}

			return _entityList.Entities;
		}

		public EntityType? Get<SerializedAccessor>(SerializedAccessor accessor, Stream? serializationSource)
		{
			if (serializationSource == null)
			{
				_logger.LogError($"{nameof(GetAll)}: {_typeName} data not found. Can't get {_typeName.ToLower()}.");
				return null;
			}

			IEnumerable<EntityType>? entities = GetAll(serializationSource);

			return entities?.FirstOrDefault(entity =>
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

		public bool Set(EntityType entity, Stream? serializationSource)
		{
			if (serializationSource == null)
			{
				_logger.LogError($"{nameof(GetAll)}: {_typeName} data not found. Can't set {_typeName.ToLower()}.");
				return false;
			}

			try
			{
				_logger.LogInformation($"{nameof(Set)}: Trying to update {_typeName.ToLower()}.");
				using (StreamWriter writer = new StreamWriter(serializationSource))
				{
					_serializer.Serialize(writer, entity);
					_logger.LogInformation($"{nameof(Set)}: Updated {_typeName.ToLower()}.");

					_entityList = new SerializedList<EntityType>();
					_logger.LogInformation($"{nameof(Set)}: Busted in-memory cache for {_typeName.ToLower()} list.");

					return true;
				}
			}
			catch
			{
				_logger.LogError($"{nameof(Set)}: Couldn't update {_typeName.ToLower()}.");
				return false;
			}
		}
	}
}
