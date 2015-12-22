using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using MPSettings.Core;
using MPSettings.Utils;

namespace MPSettings.Provider.Xml
{
    public class XmlSettingsProvider : SettingsProviderBase
    {
        private XDocument XDoc;

        private XmlSettingsProvider()
        {

        }

        public override void Initialize(IReadOnlyDictionary<string, object> namevalue)
        {
            object datastream = namevalue["dataStream"];

            if (datastream == null)
                throw new Exception(); //MP: work here

            Stream xmlStream = datastream as Stream;
            xmlStream.Seek(0, SeekOrigin.Begin);

            XDoc = XDocument.Load(xmlStream);
        }

        private XElement GetElement(SettingsPropertyName propName)
        {
            XElement elem = XDoc.Root;

            for (int index = 1; index < propName.PathParts.Length; index++)
            {
                string part = propName.PathParts[index].ToString();
                elem = elem.Element(part);
            }

            return elem;
        }

        public override IEnumerable<SettingsPropertyValue> GetPropertyValues(IEnumerable<SettingsProperty> collection)
        {
            foreach (var prop in collection)
            {
                XElement elem = GetElement(prop.PropertyName);

                if (elem != null)
                {
                    yield return new SettingsPropertyValue(prop) { SerializedValue = elem.Value };
                }
            }
        }

        public override bool HasPath(SettingsPropertyName path)
        {
            return GetElement(path) != null;
        }

        internal static XmlSettingsProvider CreateXmlSettingsProvider()
        {
            return new XmlSettingsProvider();
        }
    }
}
