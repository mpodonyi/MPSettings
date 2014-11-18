using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MPSettings.Reflection
{
    static class Reflector
    {

        internal static IEnumerable<PropertyInfo> GetProperties<T>(T obj) where T : new()
        {
            List<PropertyInfo> retval = new List<PropertyInfo>();

            IEnumerable<PropertyInfo> properties = from i in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                   where i.GetGetMethod(false) != null && i.GetSetMethod(false) != null
                                                        && i.CanRead && i.CanWrite
                                                   select i;

            foreach (PropertyInfo p in properties)
            {

                retval.Add(p);
            }

            return retval;
        }

        internal static void SetProperty<T>(T obj, PropertyInfo propInfo, object value) where T : new()
        {
            //PropertyInfo property = typeof(T).GetProperty(name);
            propInfo.SetValue(obj, value, null);
        }
    }
}
