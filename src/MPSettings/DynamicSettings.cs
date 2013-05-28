using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace MPSettings
{
    enum SettingsType
    {
        none,
        branch,
        leaf
    }


    partial class DynamicSettings : DynamicObject
    {
        private readonly SettingsImpl _settingsImpl;
        private readonly string _path;

        internal DynamicSettings(SettingsImpl settingsImpl)
        {
            _settingsImpl = settingsImpl;
        }

        private DynamicSettings(SettingsImpl settingsImpl, string path)
        {
            _path = path;
        }

        private string GetName(string name)
        {
            return (_path != null)
                ? _path + '.' + name
                : name;
        }

        
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            Tuple<SettingsType,object> val = _settingsImpl.GetValue(binder.ReturnType, GetName(binder.Name));
            if(val.Item1 == SettingsType.branch)
            {
                result = new DynamicSettings(_settingsImpl, GetName(binder.Name));
            
            }
            else if(val.Item1 == SettingsType.leaf)
            {
                result = val.Item2;
            }

            result = null;
            return false;
        }

        /// <summary> 
        /// Provides the implementation of setting a member.  Derived classes can override
        /// this method to customize behavior.  When not overridden the call site requesting the
        /// binder determines the behavior.
        /// </summary> 
        /// <param name="binder">The binder provided by the call site.</param>
        /// <param name="value">The value to set.</param> 
        /// <returns>true if the operation is complete, false if the call site should determine behavior.</returns> 
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return false;
        }

      

        /// <summary>
        /// Returns the enumeration of all dynamic member names. 
        /// </summary>
        /// <returns>The list of dynamic member names.</returns>
        public override System.Collections.Generic.IEnumerable<string> GetDynamicMemberNames()
        {
            return new string[0];
        }

        //DynamicMetaObject IDynamicMetaObjectProvider.GetMetaObject(System.Linq.Expressions.Expression parameter)
        //{
        //    return new DynamicSettingsMeta(parameter, this);
        //}
    }
}
