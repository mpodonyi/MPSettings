using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using MPSettings.Utils;

namespace MPSettings.Core
{
	public class SettingsPropertyValue
	{
		public SettingsPropertyValue(SettingsProperty property)
		{
			SettingsProperty = property;
			//PropertyValue = value;
			//PropertyFound = true;
		}

		public SettingsProperty SettingsProperty { get; private set; }

		private object _PropertyValue;
		public object PropertyValue
		{
			get
			{
				if (!_Deserialized)
				{
					_PropertyValue = GetObjectFromString(SettingsProperty.PropertyType, _SerializedValue);
					_Deserialized = true;
				}

				//if (_PropertyValue != null && !SettingsProperty.PropertyType.IsPrimitive && !(_PropertyValue is string) && !(_PropertyValue is DateTime))
				//{
				//    _ChangedSinceLastSerialized = true;
				////    _IsDirty = true;
				//}

				return _PropertyValue;
			}
			set
			{
				_PropertyValue = value;
				_ChangedSinceLastSerialized = true;
				_Deserialized = true;
				//_IsDirty = true;

			}
		}

		private string _SerializedValue;
		public string SerializedValue
		{

			get
			{
				if (_ChangedSinceLastSerialized)
				{
					_ChangedSinceLastSerialized = false;
					_SerializedValue = GetStringFromObject(SettingsProperty.PropertyType, _PropertyValue);
				}

				return _SerializedValue;
			}
			set
			{
				_SerializedValue = value;
				//_Deserialized = false;
			}
		}

		private bool _ChangedSinceLastSerialized = false;
		private bool _Deserialized = false;
		private bool _IsDirty = false;

		private static object GetObjectFromString(Type type, string attValue)
		{
			// Deal with string types 
			if (type == typeof(string) && (attValue == null || attValue.Length < 1))
				return "";

			// Return null if there is nothing to convert
			if (attValue == null || attValue.Length < 1)
				return null;

			Type type2 = Reflector.GetSimpleType(type);

			if (type2 != null)
			{
				if (type2 == typeof(DateTime))
				{
					return DateTime.Parse(attValue, DateTimeFormatInfo.InvariantInfo);
				}
				if (type2 == typeof(DateTimeOffset))
				{
					return DateTimeOffset.Parse(attValue, DateTimeFormatInfo.InvariantInfo);
				}

				return Convert.ChangeType(attValue, type2, CultureInfo.InvariantCulture);
			}
			else
			{
				XmlSerializer xs = new XmlSerializer(type);
				StringReader sr = new StringReader(attValue);

				return xs.Deserialize(sr);
			}

			//   if (val != null && !Property.PropertyType.IsAssignableFrom(val.GetType())) // is it the correct type
			//val = null; 
		}

		//internal static Type CanConvertString(Type type)
		//{
		//    type = Nullable.GetUnderlyingType(type) ?? type;
		//    return type.IsPrimitive
		//           || type == typeof(string)
		//           || type == typeof(decimal)
		//           || type == typeof(DateTime)
		//           || type == typeof(DateTimeOffset)
		//        ? type
		//        : null;
		//}



		private static string GetStringFromObject(Type type, object attValue)
		{
			// Deal with string types 
			if (type == typeof(string) || attValue == null)
				return (string)attValue;

			Type type2 = Reflector.GetSimpleType(type);

			if (type2 != null)
			{
				if (type2 == typeof(DateTime) || type2 == typeof(DateTimeOffset))
				{
					return ((IFormattable)attValue).ToString("o", DateTimeFormatInfo.InvariantInfo);
				}

				return (string)Convert.ChangeType(attValue, typeof(string), CultureInfo.InvariantCulture);
			}
			else
			{
				XmlSerializer xs = new XmlSerializer(type);
				StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);

				xs.Serialize(sw, attValue);
				return sw.ToString();
			}
		}

		//--------------------------------------------------------------------------------------------------------


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



		public static object ConvertType(object value, Type dependencyType)
		{
			if (value.GetType() == dependencyType) return value;

			if (dependencyType.GetTypeInfo().IsEnum) return Enum.Parse(dependencyType, value.ToString());

			return Convert.ChangeType(value, dependencyType);
		}


		private static string ConvertObjectToString(object propValue, Type type, bool throwOnError, SettingsSerializeAs serializeAs = SettingsSerializeAs.String)
		{
			string invariantString;
			if (serializeAs == SettingsSerializeAs.ProviderSpecific)
			{
				if (type == typeof(string) || type.GetTypeInfo().IsPrimitive)
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
							invariantString = (string) ConvertType(propValue, typeof (string));
							return invariantString;
						}
					case SettingsSerializeAs.Xml:
						{
							XmlSerializer xmlSerializer = new XmlSerializer(type);
							StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
							xmlSerializer.Serialize(stringWriter, propValue);
							invariantString = stringWriter.ToString();
							return invariantString;
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

		private object SerializePropertyValue(object _Value, SettingsSerializeAs SerializeAs)
		{
			object array=null;
			if (_Value == null)
			{
				return null;
			}
			if (SerializeAs != SettingsSerializeAs.Binary)
			{
				return SettingsPropertyValue.ConvertObjectToString(_Value, SettingsProperty.PropertyType, true, SerializeAs);
			}
			MemoryStream memoryStream = new MemoryStream();
			//try
			//{
			//	(new BinaryFormatter()).Serialize(memoryStream, this._Value);
			//	array = memoryStream.ToArray();
			//}
			//finally
			//{
			//	memoryStream.Close();
			//}
			return array;
		}




		private object Deserialize(SettingsSerializeAs SerializeAs)
		{
			object objectFromString = null;
			if (this.SerializedValue != null)
			{
				try
				{
					//if (!(this.SerializedValue is string))
					//{
					//	MemoryStream memoryStream = new MemoryStream((byte[])this.SerializedValue);
					//	try
					//	{
					//		objectFromString = (new BinaryFormatter()).Deserialize(memoryStream);
					//	}
					//	finally
					//	{
					//		memoryStream.Close();
					//	}
					//}
					//else
					//{
						objectFromString = GetObjectFromString(this.SettingsProperty.PropertyType, SerializeAs, (string)this.SerializedValue);
					//}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					try
					{
						//if (this.IsHostedInAspnet())
						//{
						//	object[] property = new object[] { this.SettingsProperty, this, exception };
						//	Type type = Type.GetType("System.Web.Management.WebBaseEvent, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true);
						//	type.InvokeMember("RaisePropertyDeserializationWebErrorEvent", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, property, CultureInfo.InvariantCulture);
						//}
					}
					catch
					{
					}
				}
				if (objectFromString != null && !this.SettingsProperty.PropertyType.GetTypeInfo().IsAssignableFrom(objectFromString.GetType().GetTypeInfo()))
				{
					objectFromString = null;
				}
			}
			//if (objectFromString == null)
			//{
			//	this._UsingDefaultValue = true;
			//	if (this.SettingsProperty.DefaultValue == null || this.SettingsProperty.DefaultValue.ToString() == "[null]")
			//	{
			//		if (!this.SettingsProperty.PropertyType.IsValueType)
			//		{
			//			return null;
			//		}
			//		return SecurityUtils.SecureCreateInstance(this.SettingsProperty.PropertyType);
			//	}
			//	if (this.SettingsProperty.DefaultValue is string)
			//	{
			//		try
			//		{
			//			objectFromString = SettingsPropertyValue.GetObjectFromString(this.SettingsProperty.PropertyType, SerializeAs, (string)this.SettingsProperty.DefaultValue);
			//		}
			//		catch (Exception exception3)
			//		{
			//			Exception exception2 = exception3;
			//			object[] name = new object[] { this.SettingsProperty.Name, exception2.Message };
			//			throw new ArgumentException(SR.GetString("Could_not_create_from_default_value", name));
			//		}
			//	}
			//	else
			//	{
			//		objectFromString = this.SettingsProperty.DefaultValue;
			//	}
			//	if (objectFromString != null && !this.SettingsProperty.PropertyType.IsAssignableFrom(objectFromString.GetType()))
			//	{
			//		object[] objArray = new object[] { this.SettingsProperty.Name };
			//		throw new ArgumentException(SR.GetString("Could_not_create_from_default_value_2", objArray));
			//	}
			//}
			if (objectFromString == null)
			{
				if (this.SettingsProperty.PropertyType != typeof(string))
				{
					try
					{
						objectFromString = SecureCreateInstance(this.SettingsProperty.PropertyType, null, false);
					}
					catch
					{
					}
				}
				else
				{
					objectFromString = "";
				}
			}
			return objectFromString;
		}


		internal static object SecureCreateInstance(Type type, object[] args, bool allowNonPublic)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
		
			return Activator.CreateInstance(type, args);
		}

		private object GetObjectFromString(Type type, SettingsSerializeAs serializeAs, string attValue)
		{
			object obj;
			if (type == typeof(string) && (attValue == null || attValue.Length < 1 || serializeAs == SettingsSerializeAs.String))
			{
				return attValue;
			}
			if (attValue == null || attValue.Length < 1)
			{
				return null;
			}
			switch (serializeAs)
			{
				case SettingsSerializeAs.String:
					{
						//TypeConverter converter = TypeDescriptor.GetConverter(type);
						//if (converter == null || !converter.CanConvertTo(typeof(string)) || !converter.CanConvertFrom(typeof(string)))
						//{
						//	object[] str = new object[] { type.ToString() };
						//	throw new ArgumentException(SR.GetString("Unable_to_convert_type_from_string", str), "type");
						//}

						//invariantString = ;
						//return invariantString;


						return ConvertType(attValue, SettingsProperty.PropertyType);
					}
				case SettingsSerializeAs.Xml:
					{
						StringReader stringReader = new StringReader(attValue);
						return (new XmlSerializer(type)).Deserialize(stringReader);
					}
				//case SettingsSerializeAs.Binary:
				//	{
				//		byte[] numArray = Convert.FromBase64String(attValue);
				//		MemoryStream memoryStream = null;
				//		try
				//		{
				//			memoryStream = new MemoryStream(numArray);
				//			obj = (new BinaryFormatter()).Deserialize(memoryStream);
				//		}
				//		finally
				//		{
				//			if (memoryStream != null)
				//			{
				//				memoryStream.Close();
				//			}
				//		}
				//		return obj;
				//	}
			}
			return null;
		}

	}
}