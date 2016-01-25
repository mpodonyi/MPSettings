using System.Collections.Generic;
using MPSettings.Core;

namespace MPSettings.Provider
{
    public interface ISettingsProvider
    {
        void Initialize(IReadOnlyDictionary<string, object> namevalue);

        IEnumerable<SettingsPropertyValue> GetPropertyValues(IEnumerable<SettingsProperty> collection);

		//bool HasPath(SettingsPropertyName path);
    }
}