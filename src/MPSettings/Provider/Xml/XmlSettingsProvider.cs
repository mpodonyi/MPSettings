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

#if NET
        private XDocument GetXDocument()
        {
            string apppath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string combinedpath = System.IO.Path.Combine(apppath, "settings.config");
            return XDocument.Load(combinedpath);
        }
#else
        private XDocument GetXDocument()
        { 
            return null;
        }
#endif

        private void Load()
        {
            XDocument xdoc = GetXDocument();
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
