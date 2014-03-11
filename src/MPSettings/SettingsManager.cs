using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using MPSettings.Provider;

namespace MPSettings
{
    public sealed class SettingsManager
    {
        private static SettingsManager _Instance = new SettingsManager();

        public static SettingsManager GetSettingsManager()
        {
            return _Instance;
        }

        // public static readonly dynamic DefaultSettings = new DynamicSettings();

        private IList<SettingsProvider> _SettingsProviders = new List<SettingsProvider>();

        //internal static SettingsImpl Instance = ((SettingsImpl)(global::System.Configuration.SettingsBase.Synchronized(new SettingsImpl())));

        //public static SettingsBase Default
        //{
        //    get
        //    {
        //        return Instance;
        //    }
        //}


        //private static T GetSettingsInternal<T>() where T : DynamicSettings, new()
        //{
        //    ISettingsAdapter adap = new DotNetSettingsAdapter(new DotNetSettingsProviderAdapter(_SettingsProviders), "");
        //    var retval = new T();
        //    retval.Initialize(adap);
        //    return retval;
        //}


        public T GetSettings<T>() where T : ISettings, new()
        {
            SettingsBridge adap = new SettingsBridge(_SettingsProviders.ToArray());
            //ISettingsAdapter adap = new DotNetSettingsAdapter(new DotNetSettingsProviderAdapter(_SettingsProviders), "");
            var retval = new T();
            retval.Initialize(adap);
            return retval;
        }

        //public static dynamic GetSettings()
        //{
        //    return GetSettingsInternal<DynamicSettings>();
        //}

        public SettingsManager AddSettingsProvider(SettingsProvider provider)
        {
            _SettingsProviders.Add(provider);
            return this;
        }
    }


}
