using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MPSettings.Reflection
{
    //internal class PropertyAccessor
    //{
    //    public Type Type { get; set; }
    //    public string Name { get; set; }
    //    public object Value { get; set; }
    //}




    static class Reflector
    {

        internal static ICollection<PropertyInfo> GetProperties<T>(T obj) where T : new()
        {
            List<PropertyInfo> retval = new List<PropertyInfo>();

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in properties)
            {
                // If not writable then cannot null it; if not readable then cannot check it's value
                if (!p.CanWrite || !p.CanRead) { continue; }

                MethodInfo mget = p.GetGetMethod(false);
                MethodInfo mset = p.GetSetMethod(false);

                // Get and set methods have to be public
                if (mget == null) { continue; }
                if (mset == null) { continue; }

                retval.Add(p);
                //retval.Add(new PropertyAccessor { Type = p.PropertyType, Name = p.Name, Value = p.GetValue(obj, null) });

                //object oldvalue = p.GetValue(obj, null);
                //object newvalue = accessor(p.PropertyType, p.Name, oldvalue);
                //if (oldvalue != newvalue)
                //{
                //    p.SetValue(obj, newvalue, null);
                //}
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
