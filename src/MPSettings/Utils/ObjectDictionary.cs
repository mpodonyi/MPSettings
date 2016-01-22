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
		private readonly Dictionary<Tuple<SettingsPropertyName, object>, object> _InnerDict = new Dictionary<Tuple<SettingsPropertyName, object>, object>();


        public void Add(SettingsPropertyName settName, object context, object obj) 
        {
            _InnerDict.Add(Tuple.Create(settName,context), obj);
        }


		internal object Get(SettingsPropertyName settName, object context)
        {
			return _InnerDict[Tuple.Create(settName, context)];
        }
    }
}
