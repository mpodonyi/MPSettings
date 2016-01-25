using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPSettings.Core;
using MPSettings.Utils;

namespace MPSettings.Dynamic
{
    class DynamicSettingsObject<TSETT> : DynamicObject
    {
        private readonly SettingsRepository _SettRepo;
        private readonly SettingsPropertyName _SettPropName;
        private readonly TSETT _Context;

        internal DynamicSettingsObject(SettingsRepository settRepo, SettingsPropertyName root, TSETT context)
        {
            _SettRepo = settRepo;
            _SettPropName = root;
            _Context = context;
        }


        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
			result = new DynamicSettingsObject<TSETT>(_SettRepo, _SettPropName + binder.Name, _Context);
			return true;



			//if (_SettRepo.HasSettingsPropertyName(_SettPropName + binder.Name))
			//{
			//	result = new DynamicSettingsObject<TSETT>(_SettRepo, _SettPropName + binder.Name, _Context);
			//	return true;
			//}
			//else
			//{
			//	result = null;
			//	return false;
			//}
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            SettingsPropertyValue obj = _SettRepo.GetPropertyValue(_Context, new SettingsProperty(_SettPropName, binder.ReturnType,null));

            if (obj != null)
            {
                result = obj.PropertyValue;
                return true;
            }

            return base.TryConvert(binder, out result);
        }

    }
}
