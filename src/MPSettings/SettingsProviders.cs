using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using MPSettings.Internal;
using MPSettings.Provider;

namespace MPSettings
{
	public static class SettingsProviders
	{
		private static readonly SettingsProviderList _Providers = new SettingsProviderList();

		public static SettingsProviderList Providers
		{
			get { return _Providers; }
		}

		private static readonly SettingsProviderInitValueCollection _InitValues = new SettingsProviderInitValueCollection();

		public static SettingsProviderInitValueCollection InitValues
		{
			get
			{
				return _InitValues;
			}
		}

		internal static SettingsProviderManager GetProviderManager()
		{
			return new SettingsProviderManager(_Providers, _InitValues);
		}
	}
}
