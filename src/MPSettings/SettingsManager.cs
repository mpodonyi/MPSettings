using MPSettings.Internal;
using MPSettings.Provider;
using MPSettings.ProviderStrategies;

namespace MPSettings
{
	public static  class SettingsManager
	{
		public static ISettBasic Create()
		{
			SettingsProviderManager spm = SettingsProviders.GetProviderManager();
			SettingsProviderStrategyCollection spsc = SettingsProviderStrategies.Strategies;

			return new SettImpl<object>(spm,spsc);
		}

		public static ISettWithContext<TSETT> Create<TSETT>() where TSETT : class
		{
			SettingsProviderManager spm = SettingsProviders.GetProviderManager();
			SettingsProviderStrategyCollection spsc = SettingsProviderStrategies.Strategies;

			return new SettImpl<TSETT>(spm,spsc);
		}


	}
}