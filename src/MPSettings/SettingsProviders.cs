using MPSettings.Provider;
using MPSettings.Provider.Xml;
using MPSettings.Util;
using System;
using System.Collections.Generic;
using System.IO;
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
                    _DefaultSettingsProvider = XmlSettingsProvider.CreateXmlSettingsProvider();
                }
                return _DefaultSettingsProvider;
            }
            set { _DefaultSettingsProvider = value; }
        }


        internal IEnumerable<ISettingsProvider> GetProviders() 
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
