using MPSettings.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MPSettings.Reflection
{
    static class Reflector
    {
        internal static bool IsSimpleType(PropertyInfo propinfo)
        {
            return true;
        }


        internal static IEnumerable<PropertyInfo> GetProperties<T>(T obj) where T : new()
        {
            return  from i in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                   where i.GetGetMethod(false) != null && i.GetSetMethod(false) != null
                                                        && i.CanRead && i.CanWrite
                                                   select i;
        }


        //internal static IEnumerable<SettingsProperty> GetPropertiesDeep<T>(T obj) where T : new()
        //{
        //    return GetPropertiesDeep("", obj);
        //}

        //private static IEnumerable<SettingsProperty> GetPropertiesDeep<T>(string pre, T obj) where T : new()
        //{
        //    foreach (var propinfo in GetProperties(obj))
        //    {
        //        if (IsSimpleType(propinfo))
        //        {

        //            yield return new SettingsProperty(propinfo.Name, propinfo.PropertyType, propinfo);
        //        }
        //        else
        //        { 
        //            yield return new SettingsProperty

                
        //        }
        //    }

        //}


        internal static void SetProperty<T>(T obj, PropertyInfo propInfo, object value) where T : new()
        {
            //PropertyInfo property = typeof(T).GetProperty(name);
            propInfo.SetValue(obj, value, null);
        }
    }
}
