using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings.Provider
{
    public abstract class SettingsProvider
    {
        protected abstract IEnumerable<SettingsPropertyValue> GetPropertyValue(SettingsContext context, IEnumerable<SettingsProperty> collection);

        protected abstract void SetPropertyValues(SettingsContext context, IEnumerable<SettingsPropertyValue> collection);
    }
}
