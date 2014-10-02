using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using sysconf = System.Configuration;

namespace MPSettings.Provider
{
    public struct SettingsNode
    {
        private readonly string _Node;

        public SettingsNode(string node)
        {
            System.Diagnostics.Contracts.Contract.Requires(!string.IsNullOrWhiteSpace(node));

            _Node = node;
        }

        private int Splitter
        {
            get
            {
                return _Node.LastIndexOf('.');
            }
        }


        public string PropName
        {
            get
            {
                var splitter = Splitter;

                return splitter == -1
                    ? _Node
                    : _Node.Substring(splitter);
            }
        }

        public string GroupName
        {
            get
            {
                var splitter = Splitter;

                return splitter == -1
                    ? _Node
                    : _Node.Substring(0, splitter);
            }
        }

        public static string GetGroupName(string node)
        {
            return new SettingsNode(node).GroupName;
        }

        public static string GetPropName(string node)
        {
            return new SettingsNode(node).PropName;
        }

        public static string GetFullName(string node, string groupname)
        {
            //node can not be empty
            return string.Join(".", groupname, node);
        }




    }



    //public class SettingsPropertyWrapper : sysconf.SettingsProperty
    //{
    //    public SettingsPropertyWrapper(string name)
    //        : base(name)
    //    { }

    //    public SettingsProperty SetProp { get; set; }

    //}

    public class DotNetSettingsProviderAdapter : SettingsProvider
    {
        private readonly sysconf.SettingsProvider SettingsProvider;


        public DotNetSettingsProviderAdapter(sysconf.SettingsProvider settingsProvider)
        {
            SettingsProvider = settingsProvider;
        }

        private sysconf.SettingsProperty FromSettingsProperty(SettingsProperty prop)
        {
            if(prop.Context.ContainsKey("_RoundtripProperty"))
            {
                return prop.Context["_RoundtripProperty"] as sysconf.SettingsProperty;
            }

            Type typeOfProperty = prop.IsUserProp
               ? typeof(sysconf.UserScopedSettingAttribute)
               : typeof(sysconf.ApplicationScopedSettingAttribute);

            sysconf.SettingsProperty setprop = new sysconf.SettingsProperty(SettingsNode.GetGroupName(prop.Name));
            setprop.PropertyType = prop.PropertyType;
            setprop.Attributes[typeOfProperty] = null;
            setprop.DefaultValue = null;
            setprop.IsReadOnly = false;

            setprop.Attributes["_RoundtripProperty"] = prop;
            prop.Context["_RoundtripProperty"] = setprop;

            return setprop;
        }

        private SettingsPropertyValue FromSettingsPropertyValue(sysconf.SettingsPropertyValue propval)
        {
            SettingsProperty prop = propval.Property.Attributes["_RoundtripProperty"] as SettingsProperty;

            if(propval.UsingDefaultValue) 
                return new SettingsPropertyValue(prop);
            
            return new SettingsPropertyValue(prop, propval.PropertyValue);
        }



        private SettingsProperty FromSettingsProperty(sysconf.SettingsProperty prop, string groupname)
        {
            if(prop.Attributes.ContainsKey("_RoundtripProperty"))
            {
                return prop.Attributes["_RoundtripProperty"] as SettingsProperty;
            }

            SettingsProperty setprop = new SettingsProperty(SettingsNode.GetFullName(prop.Name, groupname), prop.PropertyType, new Dictionary<object, object>());
            setprop.IsUserProp = prop.Attributes[typeof(sysconf.UserScopedSettingAttribute)] is sysconf.UserScopedSettingAttribute;

            setprop.Context["_RoundtripProperty"] = prop;
            prop.Attributes["_RoundtripProperty"] = setprop;

            return setprop;
        }



        



        public override IEnumerable<SettingsPropertyValue> GetPropertyValue(SettingsContext context, IEnumerable<SettingsProperty> collection)
        {
            object SettingsKey = null;
            context.TryGetValue("SettingsKey", out SettingsKey);

            

            foreach(var obj in (from i in collection group i by SettingsNode.GetGroupName(i.Name)))
            {
                sysconf.SettingsContext setcontext = new sysconf.SettingsContext();

                if(!string.IsNullOrEmpty(obj.Key))
                    setcontext["GroupName"] = obj.Key;
                if(SettingsKey != null)
                    setcontext["SettingsKey"] = SettingsKey;

                sysconf.SettingsPropertyCollection tmpcoll = new sysconf.SettingsPropertyCollection();
                foreach(var prop in obj)
                    tmpcoll.Add(FromSettingsProperty(prop));

                foreach(sysconf.SettingsPropertyValue obj2 in SettingsProvider.GetPropertyValues(setcontext, tmpcoll))
                {
                    yield return FromSettingsPropertyValue(obj2);
                }
            }

        }

        public override void SetPropertyValues(SettingsContext context, IEnumerable<SettingsPropertyValue> collection)
        {
            object SettingsKey = null;
            context.TryGetValue("SettingsKey", out SettingsKey);

            foreach(var obj in (from i in collection group i by SettingsNode.GetGroupName(i.SettingsProperty.Name)))
            {
                sysconf.SettingsContext setcontext = new sysconf.SettingsContext();
                if(!string.IsNullOrEmpty(obj.Key))
                    setcontext["GroupName"] = obj.Key;
                if(SettingsKey != null)
                    setcontext["SettingsKey"] = SettingsKey;

                //MP: work here


                sysconf.SettingsPropertyValueCollection coll = new sysconf.SettingsPropertyValueCollection();



                SettingsProvider.SetPropertyValues(setcontext, coll);
            }


        }
    }
}
