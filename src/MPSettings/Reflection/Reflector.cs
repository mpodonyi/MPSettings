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

            IEnumerable<PropertyInfo> properties = from i in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                   where i.GetGetMethod(false) != null && i.GetSetMethod(false) != null
                                                        && i.CanRead && i.CanWrite
                                                   select i;

            foreach (PropertyInfo p in properties)
            {

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
