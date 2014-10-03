using MPSettings.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MPSettings.Core
{
    public interface ISetting
    {
        void Initialize(SettingsFactory repository);
    }

    internal static class PropNameHelper
    {
        internal static string BuildName(params string[] values)
        {
            return string.Join(".", values);
        }
    }


    public class SettingsFactory
    {
        private readonly SettingsProviderList ApplicationSettingsProvider;


        //private readonly IList<SettingsProperty> ApplicationSettingsProperties;

        //private readonly IList<SettingsPropertyValue> ApplicationSettingsPropertyValues;





        internal SettingsFactory(SettingsProviderList provider)
        {
            ApplicationSettingsProvider = provider;

        }

        internal IDictionary<PropertyInfo, SettingsPropertyValue> GetPropertyValues(ICollection<PropertyInfo> propInfos)
        {
            List<SettingsProperty> properties = (from i in propInfos
                                                  select FromPropertyInfo(i)).ToList();

            List<SettingsPropertyValue> retval = new List<SettingsPropertyValue>();

            
            foreach (var provider in ApplicationSettingsProvider.GetProviders())
            {
                retval.AddRange(provider.GetPropertyValue(null, properties));

                if (properties.Count == retval.Count)
                    break;

                properties = properties.Except(retval.Select(obj => obj.SettingsProperty)).ToList();
            }

            return retval.ToDictionary(obj => obj.SettingsProperty.Context["propinfo"] as PropertyInfo, obj => obj);
        }

        private SettingsProperty FromPropertyInfo(PropertyInfo propertyInfo)
        {
            return new SettingsProperty(propertyInfo.Name, propertyInfo.PropertyType, new Dictionary<string, object> { { "propinfo", propertyInfo } });
        }

        //internal bool TryGet(Type typeOfClass, Type typeOfProperty, string name, string root, out object result)
        //{



        //}


        //protected T Get<T>(string name)
        //{
        //    var retval = ApplicationSettingsPropertyValues.FirstOrDefault(o => o.SettingsProperty.Name == name);
        //    //foreach(var provider in ApplicationSettingsProvider)
        //    //{
        //    // //   provider.



        //    //}




        //    return default(T);
        //}

        //protected void Set<T>(string name, T value)
        //{

        //}



    }






}
