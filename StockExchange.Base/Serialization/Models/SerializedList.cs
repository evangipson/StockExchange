﻿using System.Xml.Serialization;

namespace StockExchange.Base.Serialization.Models
{
	[XmlRoot("SerializedList")]
	public struct SerializedList<EntityType>() where EntityType : ISerializedEntity, new()
	{
		private List<EntityType> _entities;

		[XmlElement("SerializedItem")]
		public List<EntityType> Entities
		{
			get => _entities ??= [];
		}
	}
}
