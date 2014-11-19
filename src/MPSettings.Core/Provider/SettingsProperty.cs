﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace MPSettings.Provider
{
    public class SettingsContext : Dictionary<object, object>
    { 
    
    
    
    }

    public class SettingsProperty
    {
        public SettingsProperty(string name, Type propertyType, IDictionary<string, object> context)
        {
            Name = name;
            PropertyType = propertyType;
            Context = context;
        }

        public virtual IDictionary<string, object> Context { get; set; }
        
        public virtual string Name { get; set; }
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
            if (type == typeof(string))
                return (string)attValue;

            // Return null if there is nothing to convert
            if (attValue == null || attValue.Length < 1)
                return null;

            return Convert.ChangeType(attValue, type, CultureInfo.InvariantCulture);
        }

        private static string GetStringFromObject(Type type, object attValue)
        {
            // Deal with string types 
            if (type == typeof(string) || attValue == null )
                return (string)attValue;

            try
            {
                return (string)Convert.ChangeType(attValue, typeof(string), CultureInfo.InvariantCulture);
            }
            catch (InvalidCastException)
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
