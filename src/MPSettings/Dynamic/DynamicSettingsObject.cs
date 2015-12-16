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

        internal DynamicSettingsObject(SettingsRepository settRepo, SettingsPropertyName root)
        {
            _SettRepo = settRepo;
            _SettPropName = root;
        }

        
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_SettRepo.HasSettingsPropertyName(_SettPropName + binder.Name))
            {
                result = new DynamicSettingsObject(_SettRepo, _SettPropName + binder.Name);
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
            SettingsPropertyValue obj = _SettRepo.GetPropertyValue(new SettingsProperty(_SettPropName, binder.ReturnType, null));

            if (obj != null)
            {
                result = obj.PropertyValue;
                return true;
            }




            return base.TryConvert(binder, out result);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return base.GetDynamicMemberNames();
        }
    }
}
