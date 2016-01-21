using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using MPSettings.Provider;
using MPSettings.ProviderStrategies;

namespace MPSettings.Core
{
    internal class SettingsRepository
    {
        private readonly SettingsProviderList _spl;
        private readonly SettingsProviderStrategyCollection _spsc;
        private readonly HashSet<ISettingsProvider> IsInitialized = new HashSet<ISettingsProvider>();

        internal SettingsRepository(SettingsProviderList spl, SettingsProviderStrategyCollection spsc)
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

		//private IReadOnlyList<T> ToReadOnlyList<T>(IEnumerable<T> list)
		//{
		//	return new ReadOnlyCollection<T>(list.ToList());
		//}


		//internal IEnumerable<Tuple<ISettingsProvider, SettingsContext>> GetProviderAndContext(TSettContext obj, IEnumerable<ISettingsProvider> spl)
		//{
		//	foreach (SettingsProviderStrategyItem<TSettContext> sPS in GetSettingsProviderStrategyItems(ToReadOnlyList(spl), obj))
		//	{
		//		SettingsContext retSettCtx = new SettingsContext();
		//		foreach (var keyvalue in sPS.GetKeyValue(obj))
		//		{
		//			retSettCtx.Add(keyvalue.Key, keyvalue.Value);
		//		}

		//		yield return Tuple.Create(sPS.SettProvider, retSettCtx);

		//	}
		//}

		private IReadOnlyList<T> ToReadOnlyList<T>(IEnumerable<T> list)
		{
			return new ReadOnlyCollection<T>(list.ToList());
		}


		private IEnumerable<Tuple<ISettingsProvider, SettingsContext>> GetProviderAndContext<TSETT>(TSETT obj)
		{
			SettingsProviderStrategy<TSETT> spsc = _spsc.Get<TSETT>();
			IEnumerable<ISettingsProvider> spl = GetProviders();

			foreach (SettingsProviderStrategyItem<TSETT> sPS in spsc.GetSettingsProviderStrategyItems(ToReadOnlyList(spl), obj))
			{
				SettingsContext retSettCtx = new SettingsContext();
				foreach (var keyvalue in sPS.GetKeyValue(obj))
				{
					retSettCtx.Add(keyvalue.Key, keyvalue.Value);
				}

				yield return Tuple.Create(sPS.SettProvider, retSettCtx);

			}
		}


        internal IEnumerable<SettingsPropertyValue> GetPropertyValues<TSETT>(TSETT settinngsProp, IEnumerable<SettingsProperty> propInfos) 
        {
            List<SettingsProperty> properties = propInfos.ToList();
            List<SettingsPropertyValue> retval = new List<SettingsPropertyValue>();

            foreach (Tuple<ISettingsProvider, SettingsContext> providerTuple in GetProviderAndContext(settinngsProp))
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