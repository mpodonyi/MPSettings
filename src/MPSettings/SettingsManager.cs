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
            var obj = new T();
            ISetting settings = obj as ISetting;
            if (settings != null)
            {
                settings.Initialize(SetRepo);
                
            }
            else
            {
                foreach (var propValue in SetRepo.GetPropertyValues(Reflection.Reflector.GetProperties(obj)))
                {
                    Reflection.Reflector.SetProperty(obj,
                        propValue.SettingsProperty.Context["propinfo"] as PropertyInfo,
                        propValue.PropertyValue);
                }
            }

            return obj;
        }
       
    }


}
