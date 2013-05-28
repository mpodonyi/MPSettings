using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings
{
    public static class SettingsManager
    {
        public static dynamic GetSettings()
        {

            return new DynamicSettings(new SettingsImpl());
        }
    }
}
