using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings.Provider.Xml
{
    class XmlSettingsProvider : SettingsProvider
    {

        public override IEnumerable<SettingsPropertyValue> GetPropertyValue(SettingsContext context, IEnumerable<SettingsProperty> collection)
        {
            throw new NotImplementedException();
        }

        public override void SetPropertyValues(SettingsContext context, IEnumerable<SettingsPropertyValue> collection)
        {
            throw new NotImplementedException();
        }
    }
}
