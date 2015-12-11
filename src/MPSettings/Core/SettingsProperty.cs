using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MPSettings.Core
{
    [DebuggerDisplay("PropertyName = {PropertyName._Name}; PropertyType = {PropertyType}")]
    public class SettingsProperty
    {

        public SettingsProperty(SettingsPropertyName name, Type propertyType, IDictionary<string, object> context)
        {
            PropertyName = name;
            PropertyType = propertyType;   
            Context = context;
        }

        public virtual IDictionary<string, object> Context { get; set; }

        public virtual SettingsPropertyName PropertyName { get; set; }
        public virtual Type PropertyType { get; set; }

        //public virtual bool IsUserProp { get; set; }
    }
}
