using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MPSettings.Core;
using MPSettings.Provider;

namespace MPSettings.ProviderStrategies
{
	public abstract class SettingsProviderStrategy<TSettContext>
	{
		protected internal abstract IEnumerable<SettingsProviderStrategyItem<TSettContext>> GetSettingsProviderStrategyItems(IReadOnlyList<ISettingsProvider> spl, TSettContext sett);
	}
}
