using MPSettings.ProviderStrategies;
using MPSettings.ProviderStrategies.Implementations;

namespace MPSettings
{
	public static class SettingsProviderStrategies
	{
		private static readonly SettingsProviderStrategyCollection _Strategies = new SettingsProviderStrategyCollection();

		public static SettingsProviderStrategyCollection Strategies
		{
			get { return _Strategies; }
		}

		


		



	}
}