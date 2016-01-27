using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
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
					_PropertyValue = Converter.ConvertToObject(_SerializedValue, SettingsProperty.PropertyType);
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
					_SerializedValue = Converter.ConvertToString(_PropertyValue, true, SettingsSerializeAs.ProviderSpecific);
						
						//GetStringFromObject(SettingsProperty.PropertyType, _PropertyValue);
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
	}
}