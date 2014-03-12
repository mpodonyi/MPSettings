using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings.Provider
{
    public interface ISettings
    {
        void Initialize(SettingsBridge repository);
    }

    public class SettingsBridge
    {
        private readonly SettingsProvider[] ApplicationSettingsProvider;


        private readonly IList<SettingsProperty> ApplicationSettingsProperties;

        private readonly IList<SettingsPropertyValue> ApplicationSettingsPropertyValues;



        internal SettingsBridge(SettingsProvider[] provider)
        {
            ApplicationSettingsProvider = provider;
        }

        protected T Get<T>(string name)
        {
            var retval = ApplicationSettingsPropertyValues.FirstOrDefault(o => o.SettingsProperty.Name == name);
            foreach(var provider in ApplicationSettingsProvider)
            {
             //   provider.
            
            
            
            }




            return default(T);
        }

        protected void Set<T>(string name, T value)
        {

        }


    
    }





    
}
