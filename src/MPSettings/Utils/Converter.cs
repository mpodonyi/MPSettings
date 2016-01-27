using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MPSettings.Utils
{
	/// <summary>Determines the serialization scheme used to store application settings.</summary>
	/// <filterpriority>2</filterpriority>
	public enum SettingsSerializeAs
	{
		/// <summary>The settings property is serialized as plain text.</summary>
		String,
		/// <summary>The settings property is serialized as XML using XML serialization.</summary>
		Xml,
		/// <summary>The settings property is serialized using binary object serialization.</summary>
		Binary,
		/// <summary>The settings provider has implicit knowledge of the property or its type and picks an appropriate serialization mechanism. Often used for custom serialization.</summary>
		ProviderSpecific
	}



	internal static class Converter
	{
		private static bool CanConvertToString(Type type)
		{
			type = Nullable.GetUnderlyingType(type) ?? type;

			return type.GetTypeInfo().IsPrimitive
				   || type == typeof(string)
				   || type == typeof(decimal)
				   || type == typeof(DateTime)
				   || type == typeof(DateTimeOffset);
		}


		private static string ConvertObjectToSimpleString(object value)
		{
			if (value == null)
				return (string)null;

			if (value is string)
				return (string)value;

			Type type = value.GetType();

			if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
			{
				return ((IFormattable)value).ToString("o", DateTimeFormatInfo.InvariantInfo);
			}

			//if (dependencyType.GetTypeInfo().IsEnum) return Enum.Parse(dependencyType, value.ToString());

			return (string)Convert.ChangeType(value, typeof(string), CultureInfo.InvariantCulture);
		}

		private static string ConvertObjectToXmlString(object value)
		{
			DataContractSerializer dcSerializer = new DataContractSerializer(value.GetType());
			using (MemoryStream stream = new MemoryStream())
			using (StreamReader reader = new StreamReader(stream))
			{
				dcSerializer.WriteObject(stream, value);
				stream.Seek(0, SeekOrigin.Begin);
				return reader.ReadToEnd();
			}

			//XmlSerializer xmlSerializer = new XmlSerializer(type);
			//	StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			//	xmlSerializer.Serialize(stringWriter, propValue);
			//	invariantString = stringWriter.ToString();
			//	return invariantString;
		}

		private static object ConvertSimpleStringToObject(string value, Type type)
		{
			if (typeof(string) == type) return value;

			if (type.GetTypeInfo().IsEnum) return Enum.Parse(type, value);

			type = Nullable.GetUnderlyingType(type) ?? type;

			if (type == typeof(DateTime) || type == typeof(DateTime?))
			{
				return DateTime.Parse(value, DateTimeFormatInfo.InvariantInfo);
			}

			if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
			{
				return DateTimeOffset.Parse(value, DateTimeFormatInfo.InvariantInfo);
			}


			return Convert.ChangeType(value, type);
		}

		private static object ConvertXmlStringToObject(string value, Type type)
		{
			DataContractSerializer dcSerializer = new DataContractSerializer(type);
			using (MemoryStream stream = new MemoryStream())
			using (StreamWriter writer = new StreamWriter(stream))
			{
				writer.Write(value);
				writer.Flush();
				stream.Seek(0, SeekOrigin.Begin);
				return dcSerializer.ReadObject(stream);
			}

			//XmlSerializer xmlSerializer = new XmlSerializer(type);
			//	StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			//	xmlSerializer.Serialize(stringWriter, propValue);
			//	invariantString = stringWriter.ToString();
			//	return invariantString;
		}



		internal static string ConvertToString(object propValue, bool throwOnError, SettingsSerializeAs serializeAs = SettingsSerializeAs.ProviderSpecific)
		{
			if (propValue == null)
			{
				return null;
			}

			Type type = propValue.GetType();

			string invariantString;
			if (serializeAs == SettingsSerializeAs.ProviderSpecific)
			{
				if (CanConvertToString(type))
				{
					serializeAs = SettingsSerializeAs.String;
				}
				else
				{
					serializeAs = SettingsSerializeAs.Xml;
				}
			}
			try
			{
				switch (serializeAs)
				{
					case SettingsSerializeAs.String:
						{
							//TypeConverter converter = TypeDescriptor.GetConverter(type);
							//if (converter == null || !converter.CanConvertTo(typeof(string)) || !converter.CanConvertFrom(typeof(string)))
							//{
							//	object[] str = new object[] { type.ToString() };
							//	throw new ArgumentException(SR.GetString("Unable_to_convert_type_to_string", str), "type");
							//}
							return ConvertObjectToSimpleString(propValue);
						}
					case SettingsSerializeAs.Xml:
						{
							return ConvertObjectToXmlString(propValue);

						}
					//case SettingsSerializeAs.Binary:
					//	{
					//		MemoryStream memoryStream = new MemoryStream();
					//		try
					//		{
					//			(new BinaryFormatter()).Serialize(memoryStream, propValue);
					//			invariantString = Convert.ToBase64String(memoryStream.ToArray());
					//			return invariantString;
					//		}
					//		finally
					//		{
					//			memoryStream.Close();
					//		}
					//		break;
					//	}
				}
				return null;
			}
			catch (Exception exception)
			{
				if (throwOnError)
				{
					throw;
				}
				return null;
			}
			return invariantString;
		}


		internal static object ConvertToObject(string propValue, Type type, SettingsSerializeAs serializeAs = SettingsSerializeAs.ProviderSpecific)
		{
			if (serializeAs == SettingsSerializeAs.ProviderSpecific)
			{
				if (CanConvertToString(type))
				{
					serializeAs = SettingsSerializeAs.String;
				}
				else
				{
					serializeAs = SettingsSerializeAs.Xml;
				}
			}


			if (type == typeof (string) && (string.IsNullOrEmpty(propValue) || serializeAs == SettingsSerializeAs.String))
			{
				return propValue;
			}
			if (string.IsNullOrEmpty(propValue))
			{
				return null;
			}

			object objectFromString = null;
			try
			{
				switch (serializeAs)
				{
					case SettingsSerializeAs.String:
					{
						objectFromString = ConvertSimpleStringToObject(propValue, type);
						break;
					}
					case SettingsSerializeAs.Xml:
					{
						objectFromString = ConvertXmlStringToObject(propValue, type);
						break;
					}
				}
			}
			catch (Exception exception1)
			{

			}

			if (objectFromString != null && !type.GetTypeInfo().IsAssignableFrom(objectFromString.GetType().GetTypeInfo()))
			{
				objectFromString = null;
			}

			if (objectFromString == null)
			{
				if (type != typeof (string))
				{
					try
					{
						if (type == null)
						{
							throw new ArgumentNullException("type");
						}

						objectFromString = Activator.CreateInstance(type, null);
					}
					catch
					{
					}
				}
				else
				{
					objectFromString = ""; //MP: or null???
				}
			}
			return objectFromString;



		}



	}
}
