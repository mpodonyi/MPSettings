using MPSettings.Provider;
using MPSettings.ProviderStrategies;

namespace MPSettings
{
	public static  class SettingsManager
	{
		public static ISettBasic Create()
		{
			SettingsProviderList spl = SettingsProviders.Providers.Count == 0
				? new SettingsProviderList {SettingsProviders.Providers.DefaultProvider}
				: SettingsProviders.Providers;

			SettingsProviderStrategyCollection spsc = SettingsProviderStrategies.Strategies;

			return new SettImpl<object>(spl,spsc);
		}

		public static ISettWithContext<TSETT> Create<TSETT>() where TSETT : class
		{
			SettingsProviderList spl = SettingsProviders.Providers.Count == 0
				? new SettingsProviderList { SettingsProviders.Providers.DefaultProvider }
				: SettingsProviders.Providers;

			SettingsProviderStrategyCollection spsc = SettingsProviderStrategies.Strategies;

			return new SettImpl<TSETT>(spl,spsc);
		}


	}
}