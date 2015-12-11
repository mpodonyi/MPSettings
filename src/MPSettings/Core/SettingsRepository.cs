using System;
using System.Collections.Generic;
using System.Linq;
using MPSettings.Provider;

namespace MPSettings.Core
{
    internal class SettingsRepository 
    {
        private readonly SettingsProviderList ApplicationSettingsProvider;

        internal SettingsRepository(SettingsProviderList provider)
        {
            ApplicationSettingsProvider = provider;
        }

        private bool IsInit = false;

        private IEnumerable<ISettingsProvider> GetOrderedProviders()
        {
            return ApplicationSettingsProvider;
        }

        internal IEnumerable<SettingsPropertyValue> GetPropertyValues(IEnumerable<SettingsProperty> propInfos)
        {
            List<SettingsProperty> properties = propInfos.ToList();
            List<SettingsPropertyValue> retval = new List<SettingsPropertyValue>();


            foreach (var provider in GetOrderedProviders())
            {
                if (!IsInit)
                    provider.Initialize(SettingsProviders._InitValues); //MP: implement loading from xml file


                retval.AddRange(provider.GetPropertyValue(null, properties));

                if (properties.Count == retval.Count)
                    break;

                properties = properties.Except(retval.Select(obj => obj.SettingsProperty)).ToList();
            }

            return retval;
        }
    }
}