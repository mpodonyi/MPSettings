using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace MPSettings.YamlProvider
{
    public class YamlSettingsProvider : SettingsProvider
    {


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

        private string MyPath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.yaml");
            }
        }

        private YamlFacade yaml = null;





        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            if(yaml == null)
                yaml = new YamlFacade(MyPath);
            SettingsPropertyValueCollection propvalcoll = new SettingsPropertyValueCollection();
            foreach(SettingsProperty property in collection)
            {
                bool found;
                var obj = yaml.GetObject(property.Name, out found);

                SettingsPropertyValue val = new SettingsPropertyValue(property);
                val.PropertyValue = "test";

                propvalcoll.Add(val);
            }
            return propvalcoll;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            throw new NotImplementedException();
        }
    }
}
