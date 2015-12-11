using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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


        internal static readonly IDictionary<string, object> _InitValues = new Dictionary<string, object>();

        public static void AddProviderInitValue(string key, object value)
        {
            _InitValues.Add(key, value);
        }



    }
}
