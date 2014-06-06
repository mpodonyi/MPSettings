﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace YamlSettingsProvider
{
    public class YamlSettingsProvider : SettingsProvider
    {
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if(string.IsNullOrEmpty(name))
            {
                name = "YamlSettingsProvider";
            }
            base.Initialize(name, config);
        }

        public override string ApplicationName
        {
            get;
            set;
        }
       


        //public override string ApplicationName
        //{
        //    get
        //    {
        //        throw new NotImplementedException();
        //    }
        //    set
        //    {
        //        string xyz = value;
        //        throw new NotImplementedException();
        //    }
        //}

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
            string groupName = context["GroupName"] as string;
            Type settingsClassType = context["SettingsClassType"] as Type;
            string settingsKey = context["SettingsKey"] as string;

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
