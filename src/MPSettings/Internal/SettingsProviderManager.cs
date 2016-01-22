using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using MPSettings.Provider;

namespace MPSettings.Internal
{
	internal class SettingsProviderManager
	{
		private readonly SettingsProviderList _spl;
		private readonly SettingsProviderInitValueCollection _InitValues;


		internal SettingsProviderManager(SettingsProviderList providers, SettingsProviderInitValueCollection initValues)
		{
			_spl = providers;
			_InitValues = initValues;
		}


		internal IEnumerable<ISettingsProvider> GetProviders()
		{
			ISettingsProvider[] list = _spl.Count == 0
				? new[] { _spl.DefaultProvider }
				: _spl.ToArray();

			if (_spl._Dirty)
			{
				foreach (var settingsProvider in list)
				{
					settingsProvider.Initialize(new ReadOnlyDictionary<string, object>(_InitValues)); //MP: implement loading from xml file
				}

				_spl._Dirty = false;
			}


			return list;
		}

	}
}