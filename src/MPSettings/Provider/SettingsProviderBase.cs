using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPSettings.Core;

namespace MPSettings.Provider
{
    public abstract class SettingsProviderBase : ISettingsProvider
    {
        public virtual void Initialize(IDictionary<string, object> namevalue)
        { }

        public abstract IEnumerable<SettingsPropertyValue> GetPropertyValue(SettingsContext context, IEnumerable<SettingsProperty> collection);

        //public abstract void SetPropertyValues(SettingsContext context, IEnumerable<SettingsPropertyValue> collection);

        public abstract bool HasPath(SettingsPropertyName path);

    }
}
