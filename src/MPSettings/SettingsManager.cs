using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using MPSettings.Provider;
using MPSettings.Core;
using MPSettings.Reflection;
using System.Reflection;

namespace MPSettings
{
    public sealed class SettingsManager
    {
        private static SettingsManager _Instance = new SettingsManager();

        public static SettingsManager Instance
        {
            get
            {
                return _Instance;
            }
        }

        public T GetSettings<T>() where T : new()
        {
            //ISettingsAdapter adap = new DotNetSettingsAdapter(new DotNetSettingsProviderAdapter(_SettingsProviders), "");
            var retval = new T();
            ISetting settings = retval as ISetting;
            if (settings != null)
            {
                SettingsFactory adap = new SettingsFactory(SettingsProviders.AppSettingsProvider);
                settings.Initialize(adap);
                
            }
            else
            {
                SettingsFactory adap = new SettingsFactory(SettingsProviders.AppSettingsProvider);
                var pa = Reflection.Reflector.GetProperties(retval);
                foreach (var prop in adap.GetPropertyValues(pa))
                {
                    Reflection.Reflector.SetProperty(retval, prop.Key, prop.Value.PropertyValue);
                }
            }

            return retval;
        }
       
    }


}
