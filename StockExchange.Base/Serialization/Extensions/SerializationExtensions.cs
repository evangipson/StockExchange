using System.Runtime.Serialization;

namespace StockExchange.Base.Serialization.Extensions
{
	/// <summary>
	/// A collection of extensions for serializing and deserializing.
	/// </summary>
	internal static class SerializationExtensions
	{
		/// <summary>
		/// Serializes the provided object into an array of <see cref="byte"/>.
		/// </summary>
		/// <typeparam name="SerializedType">
		/// The type of the object to serialize.
		/// </typeparam>
		/// <param name="objectToSerialize">
		/// A required object to serialize.
		/// </param>
		/// <returns>
		/// A compressed serialized array of <see cref="byte"/>.
		/// </returns>
		/// <exception cref="Exception"></exception>
		public static byte[] Serialize<SerializedType>(this SerializedType objectToSerialize)
		{
			var serializer = new DataContractSerializer(typeof(SerializedType));
			try
			{
				using MemoryStream stream = new();
				serializer.WriteObject(stream, objectToSerialize);
				return stream.ToArray();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Deserializes the provided array of <see cref="byte"/>
		/// into an object of <typeparamref name="SerializedType"/>.
		/// Defaults to <c>null</c> if deserialization was unsuccessful.
		/// </summary>
		/// <typeparam name="SerializedType">
		/// The type of the object to deserialize.
		/// </typeparam>
		/// <param name="bytes">
		/// The array of <see cref="byte"/> to deserialize.
		/// </param>
		/// <returns>
		/// An object of <typeparamref name="SerializedType"/> if
		/// deserialization was successful. Defaults to <c>null</c>.
		/// </returns>
		/// <exception cref="Exception"></exception>
		public static SerializedType? Deserialize<SerializedType>(this byte[] bytes) where SerializedType : class, new()
		{
			var serializer = new DataContractSerializer(typeof(SerializedType));
			try
			{
				using MemoryStream stream = new(bytes);
				return serializer.ReadObject(stream) as SerializedType;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Deserializes the provided array of <see cref="byte"/>
		/// into a <see cref="string"/>. Defaults to <c>null</c>
		/// if deserialization was unsuccessful.
		/// </summary>
		/// <param name="bytes">
		/// The array of <see cref="byte"/> to deserialize.
		/// </param>
		/// <returns>
		/// A <see cref="string"/> if deserialization was successful.
		/// Defaults to <c>null</c>.
		/// </returns>
		/// <exception cref="Exception"></exception>
		public static string? Deserialize(this byte[] bytes)
		{
			var serializer = new DataContractSerializer(typeof(string));
			try
			{
				using MemoryStream stream = new(bytes);
				return serializer.ReadObject(stream)?.ToString();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Deserializes the provided array of <see cref="byte"/>
		/// into a struct of <typeparamref name="SerializedStruct"/>.
		/// </summary>
		/// <typeparam name="SerializedStruct">
		/// The type of the struct to deserialize.
		/// </typeparam>
		/// <param name="bytes">
		/// The array of <see cref="byte"/> to deserialize.
		/// </param>
		/// <returns>
		/// A struct of <typeparamref name="SerializedStruct"/> if
		/// deserialization was successful.
		/// </returns>
		/// <exception cref="Exception"></exception>
		public static SerializedStruct? DeserializeStruct<SerializedStruct>(this byte[] bytes) where SerializedStruct : struct
		{
			var serializer = new DataContractSerializer(typeof(SerializedStruct));
			try
			{
				using MemoryStream stream = new(bytes);
				return (SerializedStruct)serializer.ReadObject(stream);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Serializes an object to a file.
		/// </summary>
		/// <typeparam name="SerializedType">
		/// The type of the object to serialize.
		/// </typeparam>
		/// <param name="objectToSerialize">
		/// A required object to serialize.
		/// </param>
		/// <param name="fileName">
		/// The path of the file to serialize the <paramref name="objectToSerialize"/> into.
		/// </param>
		/// <returns>
		/// <c>true</c> if the serialization to file succeeded, <c>false</c> otherwise.
		/// </returns>
		public static bool SerializeToFile<SerializedType>(this SerializedType objectToSerialize, string fileName)
		{
			try
			{
				using BinaryWriter writer = new(File.OpenWrite(fileName));
				writer.Write(Serialize(objectToSerialize));
				writer.Flush();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static SerializedType? DeserializeFromFile<SerializedType>(string fileName) where SerializedType : class, new()
		{
			try
			{
				using BinaryReader reader = new(File.OpenRead(fileName));
				reader.BaseStream.Position = 0;
				byte[] stream = reader.ReadBytes((int)reader.BaseStream.Length);
				return stream.Deserialize<SerializedType>();
			}
			catch
			{
				return default;
			}
		}

		public static SerializedType? DeserializeStructFromFile<SerializedType>(string fileName) where SerializedType : struct
		{
			try
			{
				using BinaryReader reader = new(File.OpenRead(fileName));
				reader.BaseStream.Position = 0;
				byte[] stream = reader.ReadBytes((int)reader.BaseStream.Length);
				return stream.DeserializeStruct<SerializedType>();
			}
			catch
			{
				return default;
			}
		}
	}
}
