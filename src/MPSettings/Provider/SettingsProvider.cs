using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings.Provider
{
    public interface ISettingsProvider
    {
        void Initialize(IDictionary<string, object> namevalue);

        IEnumerable<SettingsPropertyValue> GetPropertyValue(SettingsContext context, IEnumerable<SettingsProperty> collection);

        void SetPropertyValues(SettingsContext context, IEnumerable<SettingsPropertyValue> collection);
    }


    public abstract class SettingsProvider : ISettingsProvider
    {
        public void Initialize(IDictionary<string, object> namevalue)
        { }

        public abstract IEnumerable<SettingsPropertyValue> GetPropertyValue(SettingsContext context, IEnumerable<SettingsProperty> collection);

        public abstract void SetPropertyValues(SettingsContext context, IEnumerable<SettingsPropertyValue> collection);
    }
}
