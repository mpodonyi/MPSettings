using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using MPSettings.Internals;

namespace MPSettings.Defaults
{
    public class DotNetSettingsProviderAdapter : SettingsProvider
    {
        private readonly IEnumerable<SettingsProvider> SettingsProviders;


        public DotNetSettingsProviderAdapter(IEnumerable<SettingsProvider> settingsProviders)
        {
            SettingsProviders = settingsProviders;
        }

        public override string Name
        {
            get
            {
                return "DotNetSettingsProviderAdapter";
            }
        }

        private SettingsProvider NextSettingsProvider
        {
            get
            {
                return SettingsProviders.First();
            }
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            return NextSettingsProvider.GetPropertyValues(context, collection);
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            NextSettingsProvider.SetPropertyValues(context, collection);
        }

    }
}
