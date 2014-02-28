using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using MPSettings.Defaults;
using MPSettings.Internals;

namespace MPSettings
{
    public static class SettingsManager
    {
        public static readonly dynamic DefaultSettings = new DynamicSettings();

        internal static IList<SettingsProvider> _SettingsProviders = new List<SettingsProvider>();

        //internal static SettingsImpl Instance = ((SettingsImpl)(global::System.Configuration.SettingsBase.Synchronized(new SettingsImpl())));

        //public static SettingsBase Default
        //{
        //    get
        //    {
        //        return Instance;
        //    }
        //}


        private static T GetSettingsInternal<T>() where T : DynamicSettings, new()
        {
            ISettingsAdapter adap = new DotNetSettingsAdapter(new DotNetSettingsProviderAdapter(_SettingsProviders),"");
            var retval = new T();
            retval.Initialize(adap);
            return retval;
        }


        public static T GetSettings<T>() where T : DynamicSettings, new()
        {
            return GetSettingsInternal<T>();
        }

        public static dynamic GetSettings()
        {
            return GetSettingsInternal<DynamicSettings>();
        }

        public static void AddSettingsProvider(SettingsProvider provider)
        {
            _SettingsProviders.Add(provider);
        }
    }


}
