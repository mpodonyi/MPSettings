using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using MPSettings.Provider;

namespace MPSettings.Core
{
    internal class SettingsRepository
    {
        private readonly HashSet<ISettingsProvider> IsInitialized = new HashSet<ISettingsProvider>();

        private readonly SettingsProviderList ApplicationSettingsProvider;

        internal SettingsRepository(SettingsProviderList provider)
        {
            ApplicationSettingsProvider = provider;
        }

        private IEnumerable<ISettingsProvider> GetOrderedProviders()
        {
            foreach (var provider in ApplicationSettingsProvider)
            {
                if (!IsInitialized.Contains(provider))
                {
                    provider.Initialize(new ReadOnlyDictionary<string, object>(SettingsProviders._InitValues)); //MP: implement loading from xml file
                    IsInitialized.Add(provider);
                }

                yield return provider;
            }
        }

        internal IEnumerable<SettingsPropertyValue> GetPropertyValues(IEnumerable<SettingsProperty> propInfos)
        {
            List<SettingsProperty> properties = propInfos.ToList();
            List<SettingsPropertyValue> retval = new List<SettingsPropertyValue>();


            foreach (var provider in GetOrderedProviders())
            {
                retval.AddRange(provider.GetPropertyValues(properties));

                if (properties.Count == retval.Count)
                    break;

                properties = properties.Except(retval.Select(obj => obj.SettingsProperty)).ToList();
            }

            return retval;
        }

        internal SettingsPropertyValue GetPropertyValue(SettingsProperty propInfo)
        {
            foreach (var provider in GetOrderedProviders())
            {
                var retval = provider.GetPropertyValues(new[] { propInfo }).SingleOrDefault();

                if (retval != null)
                    return retval;
            }

            return null;
        }

        internal bool HasSettingsPropertyName(SettingsPropertyName settPropName)
        {
            foreach (var provider in GetOrderedProviders())
            {
                if (provider.HasPath(settPropName))
                    return true;
            }

            return false;
        }
    }
}