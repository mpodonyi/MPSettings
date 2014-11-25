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


   


    internal class SettingsRepository : ISettingsRepository
    {
        private readonly SettingsProviderList ApplicationSettingsProvider;


        //private readonly IList<SettingsProperty> ApplicationSettingsProperties;

        //private readonly IList<SettingsPropertyValue> ApplicationSettingsPropertyValues;





        internal SettingsRepository(SettingsProviderList provider)
        {
            ApplicationSettingsProvider = provider;
            
        }

        internal IEnumerable<SettingsPropertyValue> GetPropertyValues(IEnumerable<SettingsProperty> propInfos)
        {
            List<SettingsProperty> properties = propInfos.ToList();

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
