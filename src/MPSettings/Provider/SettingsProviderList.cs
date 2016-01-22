using System.Collections.Generic;
using MPSettings.Provider.Xml;

namespace MPSettings.Provider
{
	public class SettingsProviderList : List<ISettingsProvider>
	{
		private ISettingsProvider _DefaultProvider;

		public ISettingsProvider DefaultProvider
		{
			get
			{
				if (_DefaultProvider == null)
				{
					_DefaultProvider = XmlSettingsProvider.CreateXmlSettingsProvider();
				}
				return _DefaultProvider;
			}
			set { _DefaultProvider = value; }
		}

		internal volatile bool _Dirty = true;

		public void Reload()
		{
			_Dirty = true;
		}


	}
}