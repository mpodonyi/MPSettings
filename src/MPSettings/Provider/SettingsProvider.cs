using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings.Provider
{
    public abstract class SettingsProvider
    {
        protected abstract SettingsPropertyValue GetPropertyValue(SettingsContext context, SettingsProperty collection);

        protected abstract void SetPropertyValues(SettingsContext context, SettingsPropertyValue collection);
    }
}
