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
        private readonly SettingsProviderList _spl;
        private readonly SettingsProviderStrategyCollectionDictionary _spsc;
        private readonly HashSet<ISettingsProvider> IsInitialized = new HashSet<ISettingsProvider>();

        internal SettingsRepository(SettingsProviderList spl, SettingsProviderStrategyCollectionDictionary spsc)
        {
            _spsc = spsc;
            _spl = spl;
        }


        private IEnumerable<SettingsProperty> GetWithContext(IEnumerable<SettingsProperty> props, SettingsContext context)
        {
            foreach (SettingsProperty prop in props)
            {
                prop.Context = context;
                yield return prop;
            }
        }

        private IEnumerable<ISettingsProvider> GetProviders()
        {
            foreach (var prov in _spl)
            {
                if (!IsInitialized.Contains(prov))
                {
                    prov.Initialize(new ReadOnlyDictionary<string, object>(SettingsProviders._InitValues)); //MP: implement loading from xml file
                    IsInitialized.Add(prov);
                }

                yield return prov;
            }
        }


        internal IEnumerable<SettingsPropertyValue> GetPropertyValues<TSETT>(TSETT settinngsProp, IEnumerable<SettingsProperty> propInfos) 
        {
            SettingsProviderStrategyCollectionBase<TSETT> spsc = _spsc.GetSPSC<TSETT>();
            

            List<SettingsProperty> properties = propInfos.ToList();
            List<SettingsPropertyValue> retval = new List<SettingsPropertyValue>();




            foreach (Tuple<ISettingsProvider, SettingsContext> providerTuple in spsc.GetProviderAndContext(settinngsProp, GetProviders()))
            {
                retval.AddRange(providerTuple.Item1.GetPropertyValues(GetWithContext(properties, providerTuple.Item2)));

                if (properties.Count == retval.Count)
                    break;

                properties = properties.Except(retval.Select(obj => obj.SettingsProperty)).ToList();
            }


            //foreach (var provider in GetOrderedProviders())
            //{
            //    retval.AddRange(provider.GetPropertyValues(properties));

            //    if (properties.Count == retval.Count)
            //        break;

            //    properties = properties.Except(retval.Select(obj => obj.SettingsProperty)).ToList();
            //}

            return retval;
        }

        internal SettingsPropertyValue GetPropertyValue<TSETT>(TSETT settinngsProp, SettingsProperty propInfo)
        {
            return GetPropertyValues(settinngsProp, new[] {propInfo}).FirstOrDefault();
        }

        internal bool HasSettingsPropertyName(SettingsPropertyName settPropName)
        {
            foreach (var provider in GetProviders())
            {
                if (provider.HasPath(settPropName))
                    return true;
            }

            return false;
        }
    }
}