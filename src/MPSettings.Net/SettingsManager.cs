using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPSettings.Provider;

namespace MPSettings
{

    public static class SettingsManagerExtensions
    {

        public static SettingsManager AddSettingsProvider(this SettingsManager obj, System.Configuration.SettingsProvider provider)
        {
            return obj.AddSettingsProvider(new DotNetSettingsProviderAdapter(provider));
        }


    }
}
