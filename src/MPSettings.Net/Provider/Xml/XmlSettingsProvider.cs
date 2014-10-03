using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MPSettings.Provider.Xml
{
    class XmlSettingsProvider : SettingsProvider
    {
        public override void Initialize(IDictionary<string, object> namevalue)
        {
            Path = namevalue["path"] as string;
        }

        private string Path
        {
            get;
            set;
        }

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
