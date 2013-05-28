using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MPSettings
{
    class SettingsImpl:SettingsBase
    {

        internal Tuple<SettingsType, object> GetValue(Type type, string name)
        {
            var value = base[name];
            throw new NotImplementedException();
        }
    }
}
