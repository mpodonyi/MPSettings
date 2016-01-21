using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPSettings.Provider;

namespace MPSettings.ProviderStrategies.Implementations
{
	class SimpleSettingsProviderStrategy<TSETT> : SettingsProviderStrategy<TSETT>
	{
		protected internal override IEnumerable<SettingsProviderStrategyItem<TSETT>> GetSettingsProviderStrategyItems(IReadOnlyList<ISettingsProvider> spl, TSETT sett)
		{
			foreach (ISettingsProvider settProvider in spl)
			{
				yield return new SettingsProviderStrategyItem<TSETT>(settProvider, typeof(TSETT));
				yield return new SettingsProviderStrategyItem<TSETT>(settProvider);
			}
		}
	}

	//public class DefaultSettingsProviderStrategyCollection : SettingsProviderStrategy<UserSettingsContext>
	//{
	//	protected override IEnumerable<SettingsProviderStrategyItem<UserSettingsContext>> GetSettingsProviderStrategyItems(IReadOnlyList<ISettingsProvider> spl, UserSettingsContext obj)
	//	{
	//		foreach (ISettingsProvider settProvider in spl)
	//		{
	//			yield return new SettingsProviderStrategyItem<UserSettingsContext>(settProvider, sett => sett.UserId, sett => sett.Domain);
	//			yield return new SettingsProviderStrategyItem<UserSettingsContext>(settProvider, sett => sett.Domain);
	//			yield return new SettingsProviderStrategyItem<UserSettingsContext>(settProvider);
	//		}
	//	}
	//}


	//public class UserSettingsContext
	//{
	//	public string UserId { get; set; }
	//	public int Domain { get; set; }


	//}


}
