using System;
using System.Collections.Generic;
using MPSettings.ProviderStrategies.Implementations;

namespace MPSettings.ProviderStrategies
{
	public class SettingsProviderStrategyCollection
	{
		private readonly IDictionary<Type, SettingsProviderStrategy<object>> _SettingsProviderStrategies = new Dictionary<Type, SettingsProviderStrategy<object>>();

		public void Add<TSETT>(Type sett, SettingsProviderStrategy<object> settingsProviderStrategy)
		{
			_SettingsProviderStrategies.Add(typeof(TSETT), settingsProviderStrategy);
		}


		public SettingsProviderStrategy<TSETT> Get<TSETT>()
		{
			SettingsProviderStrategy<object> retSettingsProviderStrategy;

			if (_SettingsProviderStrategies.TryGetValue(typeof(TSETT), out retSettingsProviderStrategy))
			{
				return retSettingsProviderStrategy as SettingsProviderStrategy<TSETT>;
			}

			return new SimpleSettingsProviderStrategy<TSETT>();
		}


	}
}