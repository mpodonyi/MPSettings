using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings.Provider
{
    public class SettingsContext : Dictionary<object, object>
    { 
    
    
    
    }

    public class SettingsProperty
    {
        public SettingsProperty(string name, Type propertyType, IDictionary<object, object> context)
        { 
            
        
        }

        public virtual IDictionary<string, object> Context { get; set; }
        
        public virtual string Name { get; set; }
        public virtual Type PropertyType { get; set; }

        public virtual bool IsUserProp { get; set; }
    }

    public class SettingsPropertyValue
    {
        public SettingsPropertyValue(SettingsProperty property, object value)
        {
            SettingsProperty = property;
            PropertyValue = value;
            PropertyFound = true;
        }

        public SettingsPropertyValue(SettingsProperty property)
        {
            SettingsProperty = property;
            PropertyFound = false;
        }


        public SettingsProperty SettingsProperty { get; private set; }

        public object PropertyValue { get; private set; }

        public bool PropertyFound { get; private set; }
    }
}
