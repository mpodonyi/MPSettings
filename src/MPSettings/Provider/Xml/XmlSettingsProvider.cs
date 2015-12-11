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

        public override void Initialize(IDictionary<string, object> namevalue)
        {
            object datastream = namevalue["dataStream"];

            if(datastream ==null)
                throw new Exception(); //MP: work here

            using (Stream xmlStream = datastream as Stream)
            {
                XDoc = XDocument.Load(xmlStream);
            }
        }

        public override IEnumerable<SettingsPropertyValue> GetPropertyValue(SettingsContext context, IEnumerable<SettingsProperty> collection)
        {
            var root = XDoc.Root;
            foreach (var prop in collection)
            {
                XElement elem=root ;

                for (int index = 1; index < prop.PropertyName.PathParts.Length; index++)
                {
                    string part = prop.PropertyName.PathParts[index];
                    elem = elem.Element(part);
                }

                if (elem != null)
                {
                    yield return new SettingsPropertyValue(prop) {SerializedValue = elem.Value};
                }
            }
        }
       
        internal static XmlSettingsProvider CreateXmlSettingsProvider()
        {
            return new XmlSettingsProvider();
        }
    }
}
