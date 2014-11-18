using MPSettings.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace MPSettings.Provider.Xml
{
    public class XmlSettingsProvider : SettingsProvider
    {
        private readonly XDocument XDoc;

        private XmlSettingsProvider(Stream xmlStream)
        {
            XDoc = XDocument.Load(xmlStream);
        }

        public override IEnumerable<SettingsPropertyValue> GetPropertyValue(SettingsContext context, IEnumerable<SettingsProperty> collection)
        {
            var root = XDoc.Root;
            foreach (var prop in collection)
            {
                yield return new SettingsPropertyValue(prop) { SerializedValue = root.Element(prop.Name).Value };
            }
        }

        public override void SetPropertyValues(SettingsContext context, IEnumerable<SettingsPropertyValue> collection)
        {
            throw new NotImplementedException();
        }

        internal static XmlSettingsProvider CreateXmlSettingsProvider()
        {
            Stream stream = PathHelper.GetApplicationFileStream("settings.config", true);
            return new XmlSettingsProvider(stream);
        }
    }
}
