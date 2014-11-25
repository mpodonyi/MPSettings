using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace MPSettings.Provider
{
    public class SettingsContext : Dictionary<object, object>
    {



    }


    public struct SettingsPropertyName
    {
        private readonly string _Name;

        public SettingsPropertyName(string name)
        {
            _Name = name.Trim().Trim('.');
        }


        public static implicit operator SettingsPropertyName(string name)
        {
            return new SettingsPropertyName(name);
        }

        // overload operator + 
        public static SettingsPropertyName operator +(SettingsPropertyName a, SettingsPropertyName b)
        {
            return new SettingsPropertyName(a._Name + '.' + b._Name);
        }
    
    }


    public class SettingsProperty
    {

        public SettingsProperty(SettingsPropertyName name, Type propertyType, IDictionary<string, object> context)
        {
            Name = name;
            PropertyType = propertyType;
            Context = context;
        }

        public virtual IDictionary<string, object> Context { get; set; }

        public virtual SettingsPropertyName Name { get; set; }
        public virtual Type PropertyType { get; set; }

        //public virtual bool IsUserProp { get; set; }
    }

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

            Type type2 = CanConvertString(type);

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

        private static Type CanConvertString(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            return type.IsPrimitive
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                ? type
                : null;
        }



        private static string GetStringFromObject(Type type, object attValue)
        {
            // Deal with string types 
            if (type == typeof(string) || attValue == null)
                return (string)attValue;

            Type type2 = CanConvertString(type);

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


        //private object Deserialize()
        //{
        //    object val = null;
        //    ////////////////////////////////////////////// 
        //    /// Step 1: Try creating from Serailized value
        //    if (SerializedValue != null)
        //    {
        //        try
        //        {
        //            if (SerializedValue is string)
        //            {
        //                val = GetObjectFromString(Property.PropertyType, Property.SerializeAs, (string)SerializedValue);
        //            }
        //            else
        //            {
        //                MemoryStream ms = new System.IO.MemoryStream((byte[])SerializedValue);
        //                try
        //                {
        //                    val = (new BinaryFormatter()).Deserialize(ms);
        //                }
        //                finally
        //                {
        //                    ms.Close();
        //                }
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            try
        //            {
        //                if (IsHostedInAspnet())
        //                {
        //                    object[] args = new object[] { Property, this, exception };

        //                    const string webBaseEventTypeName = "System.Web.Management.WebBaseEvent, " + AssemblyRef.SystemWeb;

        //                    Type type = Type.GetType(webBaseEventTypeName, true);

        //                    type.InvokeMember("RaisePropertyDeserializationWebErrorEvent",
        //                        BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod,
        //                        null, null, args, CultureInfo.InvariantCulture);
        //                }
        //            }
        //            catch
        //            {
        //            }
        //        }

        //        if (val != null && !Property.PropertyType.IsAssignableFrom(val.GetType())) // is it the correct type
        //            val = null;
        //    }

        //    ////////////////////////////////////////////// 
        //    /// Step 2: Try creating from default value
        //    if (val == null)
        //    {
        //        _UsingDefaultValue = true;
        //        if (Property.DefaultValue == null || Property.DefaultValue.ToString() == "[null]")
        //        {
        //            if (Property.PropertyType.IsValueType)
        //                return SecurityUtils.SecureCreateInstance(Property.PropertyType);
        //            else
        //                return null;
        //        }
        //        if (!(Property.DefaultValue is string))
        //        {
        //            val = Property.DefaultValue;
        //        }
        //        else
        //        {
        //            try
        //            {
        //                val = GetObjectFromString(Property.PropertyType, Property.SerializeAs, (string)Property.DefaultValue);
        //            }
        //            catch (Exception e)
        //            {
        //                throw new ArgumentException(SR.GetString(SR.Could_not_create_from_default_value, Property.Name, e.Message));
        //            }
        //        }
        //        if (val != null && !Property.PropertyType.IsAssignableFrom(val.GetType())) // is it the correct type
        //            throw new ArgumentException(SR.GetString(SR.Could_not_create_from_default_value_2, Property.Name));
        //    }

        //    ////////////////////////////////////////////// 
        //    /// Step 3: Create a new one by calling the parameterless constructor 
        //    if (val == null)
        //    {
        //        if (Property.PropertyType == typeof(string))
        //        {
        //            val = "";
        //        }
        //        else
        //        {
        //            try
        //            {
        //                val = SecurityUtils.SecureCreateInstance(Property.PropertyType);
        //            }
        //            catch { }
        //        }
        //    }

        //    return val;
        //}
    }
}
