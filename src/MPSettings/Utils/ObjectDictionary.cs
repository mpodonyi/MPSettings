using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPSettings.Core;

namespace MPSettings.Utils
{
    internal class ObjectDictionary
    {
        private readonly Dictionary<SettingsPropertyName, object> _InnerDict = new Dictionary<SettingsPropertyName, object>();


        public void Add(SettingsPropertyName settName, object obj) 
        {
            _InnerDict.Add(settName, obj);
        }


        internal object Get(SettingsPropertyName settName)
        {
            return _InnerDict[settName];
        }
    }
}
