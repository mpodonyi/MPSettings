using System.Collections.Generic;
using MPSettings.Core;

namespace MPSettings.Provider
{
    public interface ISettingsProvider
    {
        void Initialize(IDictionary<string, object> namevalue);

        IEnumerable<SettingsPropertyValue> GetPropertyValue(SettingsContext context, IEnumerable<SettingsProperty> collection);

        bool HasPath(SettingsPropertyName path);
    }
}