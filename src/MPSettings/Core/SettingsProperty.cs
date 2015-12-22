using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MPSettings.Core
{
    [DebuggerDisplay("PropertyName = {PropertyName._Name}; PropertyType = {PropertyType}")]
    public class SettingsProperty
    {

        public SettingsProperty(SettingsPropertyName name, Type propertyType, SettingsContext context)
        {
            PropertyName = name;
            PropertyType = propertyType;   
            Context = context;
        }


        internal protected virtual IDictionary<string, object> InternalContext { get; set; }



        public virtual SettingsPropertyName PropertyName { get; private set; }
        public virtual Type PropertyType { get; private set; }

        public virtual SettingsContext Context { get; internal set; }


        //public virtual bool IsUserProp { get; set; }
    }
}
