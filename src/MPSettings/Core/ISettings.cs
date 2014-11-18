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
        void Initialize(ISettingsRepository repository);
    }

    public interface ISettingsRepository 
    {
        T Get<T>(string name);

        void Set<T>(string name, T value);
    }


    internal static class PropNameHelper
    {
        internal static string BuildName(params string[] values)
        {
            return string.Join(".", values);
        }
    }


    internal class SettingsRepository : ISettingsRepository
    {
        private readonly SettingsProviderList ApplicationSettingsProvider;


        //private readonly IList<SettingsProperty> ApplicationSettingsProperties;

        //private readonly IList<SettingsPropertyValue> ApplicationSettingsPropertyValues;





        internal SettingsRepository(SettingsProviderList provider)
        {
            ApplicationSettingsProvider = provider;

        }

        internal IEnumerable<SettingsPropertyValue> GetPropertyValues(IEnumerable<PropertyInfo> propInfos)
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

            return retval;
        }

        private SettingsProperty FromPropertyInfo(PropertyInfo propertyInfo)
        {
            //check that not in cache
            return new SettingsProperty(propertyInfo.Name, propertyInfo.PropertyType, new Dictionary<string, object> { { "propinfo", propertyInfo } });
        }


        



        public T Get<T>(string name)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string name, T value)
        {
            throw new NotImplementedException();
        }




    
    }






}
