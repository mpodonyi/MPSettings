using MPSettings.Provider;
using MPSettings.Provider.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings
{
    public class SettingsProviderList 
    {
        private ISettingsProvider _DefaultSettingsProvider;

        public ISettingsProvider DefaultSettingsProvider
        {
            get
            {
                if (_DefaultSettingsProvider == null)
                {
                    _DefaultSettingsProvider = new XmlSettingsProvider();
                }
                return _DefaultSettingsProvider;
            }
            set { _DefaultSettingsProvider = value; }
        }


        internal IEnumerable<ISettingsProvider> GetProviders() //MP: startegy maybe here
        {
            yield return DefaultSettingsProvider;
        }
    
    }


    public static class SettingsProviders
    {
        private static readonly SettingsProviderList _AppSettingsProvider = new SettingsProviderList();

        public static SettingsProviderList AppSettingsProvider 
        {
            get { return _AppSettingsProvider; }
        }



        

      
    }
}
