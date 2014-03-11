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
        public SettingsProperty(string name, Type propertyType, IDictionary<string, object> context)
        { 
            
        
        }

        public virtual IDictionary<string, object> Context { get; set; }
        
        public virtual string Name { get; set; }
        public virtual Type PropertyType { get; set; }
    }

    public class SettingsPropertyValue
    {
        public SettingsPropertyValue(SettingsProperty property, object value)
        {


        }

        public SettingsPropertyValue(SettingsProperty property)
        {


        }

        public object PropertyValue { get; private set; }

        public bool PropertyFound { get; private set; }
    }
}
