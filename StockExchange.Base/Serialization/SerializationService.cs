using System.Xml.Serialization;
using Microsoft.Extensions.Logging;

using StockExchange.Base.DependencyInjection;
using StockExchange.Base.Serialization;
using StockExchange.Base.Serialization.Interfaces;

namespace StockExchange.Base.Services
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

		public IEnumerable<EntityType> GetAll(string xmlFilePath)
		{
			_logger.LogInformation($"{nameof(Get)}: Deserializing every {_typeName.ToLower()}.");
			using (StreamReader reader = new StreamReader(xmlFilePath))
			{
				_entityList = _serializer.Deserialize(reader) as SerializedList<EntityType>? ?? new SerializedList<EntityType>();
			}

			return _entityList.Entities;
		}

		public EntityType Get<SerializedAccessor>(SerializedAccessor accessor, string xmlFilePath)
		{
			IEnumerable<EntityType> entities = GetAll(xmlFilePath);

			var entity = entities.FirstOrDefault(entity =>
			{
				var entityProperties = typeof(EntityType).GetProperties();
				foreach (var property in entityProperties)
				{
					if (property.GetValue(entity)?.ToString() == accessor?.ToString())
					{
						_logger.LogInformation($"{nameof(Get)}: Deserialized {_typeName.ToLower()} using \"{accessor}\".");
						return true;
					}
				}
				_logger.LogWarning($"{nameof(Get)}: Did not find a {_typeName.ToLower()} to deserialize using \"{accessor}\".");
				return false;
			});

			return entity ?? new();
		}

		public bool Set(EntityType entity, string xmlFilePath)
		{
			try
			{
				_logger.LogInformation($"{nameof(Set)}: Trying to serialize {_typeName.ToLower()}.");
				using (StreamWriter writer = new StreamWriter(xmlFilePath))
				{
					_serializer.Serialize(writer, entity);
					_logger.LogInformation($"{nameof(Set)}: Serialized {_typeName.ToLower()}.");
					return true;
				}
			}
			catch
			{
				_logger.LogError($"{nameof(Set)}: Couldn't serialize {_typeName.ToLower()}.");
				return false;
			}
		}
	}
}
