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
        private static bool IsSimpleType(PropertyInfo propinfo)
        {
            return MPSettings.Provider.SettingsPropertyValue.CanConvertString(propinfo.PropertyType) != null;
        }


        private static IEnumerable<PropertyInfo> GetProperties(Type type) 
        {
          


            return from i in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                   where i.GetGetMethod(false) != null && i.GetSetMethod(false) != null
                                                        && i.CanRead && i.CanWrite
                                                   select i;
        }

        internal static T GetSettingsProperties<T>(Func< IEnumerable<SettingsProperty>,IEnumerable<SettingsPropertyValue>> func) where T : new()
        {
            List<SettingsProperty> settproplist = new List<SettingsProperty>();
            T temp = new T();
            NewMethod(temp, settproplist, "");

            foreach (SettingsPropertyValue propValue in func(settproplist))
            {
                SetProperty(
                    propValue.SettingsProperty.Context["_propobject"],
                    propValue.SettingsProperty.Context["_propinfo"] as PropertyInfo,
                    propValue.PropertyValue);
            }

            return temp;
        }


        private static void NewMethod<T>(T obj, List<SettingsProperty> settproplist, string pre) where T : new()
        {
            foreach (var propinfo in GetProperties(obj.GetType()))
            {
                if (IsSimpleType(propinfo))
                {
                    settproplist.Add(SettingsPropertyCreateFrom(propinfo, obj, pre));
                }
                else
                {
                    Type declType = propinfo.PropertyType;
                    //if (declType.GetConstructor(Type.Missing) == null)
                    //    throw new Exception("Can't find public default Constuctor");

                    object obj2 = Activator.CreateInstance(declType);
                    SetProperty(obj, propinfo, obj2);

                    NewMethod(obj2, settproplist, pre + propinfo.Name);
                }
            }
        }

        private static SettingsProperty SettingsPropertyCreateFrom<T>(PropertyInfo propertyInfo, T propertyObject, string pre) where T : new()
        {
            return new SettingsProperty(((SettingsPropertyName)pre) + propertyInfo.Name, propertyInfo.PropertyType, new Dictionary<string, object> { { "_propinfo", propertyInfo }, { "_propobject", propertyObject } });
        }

        private static void SetProperty<T>(T obj, PropertyInfo propInfo, object value) where T : new()
        {
            //PropertyInfo property = typeof(T).GetProperty(name);
            propInfo.SetValue(obj, value, null);
        }
    }
}
