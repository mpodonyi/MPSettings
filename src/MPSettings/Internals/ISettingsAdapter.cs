using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings.Internals
{
    interface ISettingsAdapter
    {
        object this[string propertyName] { get; set; }
        void RegisterProperty(string name, Type type);
    }
}
