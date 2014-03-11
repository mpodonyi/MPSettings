using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MPSettings.Provider
{
    public class DotNetSettingsProviderAdapter : SettingsProvider
    {
        private readonly System.Configuration.SettingsProvider SettingsProvider;


        public DotNetSettingsProviderAdapter(System.Configuration.SettingsProvider settingsProvider)
        {
            SettingsProvider = settingsProvider;
        }


        protected override SettingsPropertyValue GetPropertyValue(SettingsContext context, SettingsProperty collection)
        {
            throw new NotImplementedException();
        }

        protected override void SetPropertyValues(SettingsContext context, SettingsPropertyValue collection)
        {
            throw new NotImplementedException();
        }
    }
}
