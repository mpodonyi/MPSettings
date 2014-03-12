using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace MPSettings.Provider
{
    public class SettingsPropertyWrapper : System.Configuration.SettingsProperty
    {
        public SettingsPropertyWrapper(string name)
            : base(name)
        { }

        public SettingsProperty SetProp { get; set; }

    }

    public class DotNetSettingsProviderAdapter : SettingsProvider
    {
        private readonly System.Configuration.SettingsProvider SettingsProvider;


        public DotNetSettingsProviderAdapter(System.Configuration.SettingsProvider settingsProvider)
        {
            SettingsProvider = settingsProvider;
        }

        private string GetPropName(string name)
        {
            string[] splitted = name.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            return splitted.Length > 0
                ? splitted[splitted.Length - 1]
                : null;
        }

        private string GetPropPath(string name)
        {
            return null;
        }

        private SettingsPropertyCollection ToSettingsPropertyCollection(IEnumerable<SettingsProperty> collection)
        {
            SettingsPropertyCollection retval = new SettingsPropertyCollection();
            foreach(var prop in collection)
            {
                SettingsPropertyWrapper setprop = new SettingsPropertyWrapper(GetPropName(prop.Name));
                setprop.SetProp = prop;
                setprop.PropertyType = prop.PropertyType;
                
                retval.Add(setprop);
            }

            return retval;
        }

        private IEnumerable<SettingsPropertyValue> FromSettingsPropertyValueCollection(SettingsPropertyValueCollection collection)
        {
            foreach(System.Configuration.SettingsPropertyValue obj in collection)
            {
                var wrappedprop = obj.Property as SettingsPropertyWrapper;

                SettingsPropertyValue propValue=new SettingsPropertyValue(wrappedprop.SetProp,obj.PropertyValue);

                yield return propValue;
            }
        }


        protected override IEnumerable<SettingsPropertyValue> GetPropertyValue(SettingsContext context, IEnumerable<SettingsProperty> collection)
        {
            System.Configuration.SettingsContext setcontext=new System.Configuration.SettingsContext();

            return FromSettingsPropertyValueCollection(SettingsProvider.GetPropertyValues(setcontext,ToSettingsPropertyCollection(collection)));
        }

        protected override void SetPropertyValues(SettingsContext context, IEnumerable<SettingsPropertyValue> collection)
        {
            SettingsContext cont = new SettingsContext();



            SettingsProvider.SetPropertyValues(cont,)


            throw new NotImplementedException();
        }
    }
}
