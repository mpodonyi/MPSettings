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
        private static readonly Lazy<SettingsManager> lazy = new Lazy<SettingsManager>(() => new SettingsManager());

        public static SettingsManager Instance { get { return lazy.Value; } }

        private readonly SettingsRepository SetRepo;

        private SettingsManager()
        {
            SetRepo = new SettingsRepository(SettingsProviders.AppSettingsProvider);
        }



        public T GetSettings<T>() where T : new()
        {
            //ISettingsAdapter adap = new DotNetSettingsAdapter(new DotNetSettingsProviderAdapter(_SettingsProviders), "");
            
            
            if (typeof(ISetting).IsAssignableFrom(typeof(T)))
            {
                T retval = new T();
                ISetting settings = retval as ISetting;

                settings.Initialize(SetRepo);

                return retval;
            }
            else
            {
                return Reflection.Reflector.GetSettingsProperties<T>(o=>SetRepo.GetPropertyValues(o));
            }
        }

     

    }


}
