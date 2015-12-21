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
    class DynamicSettingsObject : DynamicObject
    {
        private readonly SettingsRepository _SettRepo;
        private readonly SettingsPropertyName _SettPropName;
        private readonly SettingsContext _Context;

        internal DynamicSettingsObject(SettingsRepository settRepo, SettingsPropertyName root, SettingsContext context)
        {
            _SettRepo = settRepo;
            _SettPropName = root;
            _Context = context;
        }


        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_SettRepo.HasSettingsPropertyName(_SettPropName + binder.Name))
            {
                result = new DynamicSettingsObject(_SettRepo, _SettPropName + binder.Name, _Context);
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            SettingsPropertyValue obj = _SettRepo.GetPropertyValue(new SettingsProperty(_SettPropName, binder.ReturnType, new SettingsContext(_Context)));

            if (obj != null)
            {
                result = obj.PropertyValue;
                return true;
            }

            return base.TryConvert(binder, out result);
        }

    }
}
