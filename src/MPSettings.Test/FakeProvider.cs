using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSettings.Test
{
    public class FakeProvider: SettingsProvider
    {

        private string _appName = string.Empty;


        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize("FakeProvider", config);
        }

        public override string ApplicationName
        {
           
            get
            {
                return this._appName;
            }
          
            set
            {
                this._appName = value;
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            throw new NotImplementedException();
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            throw new NotImplementedException();
        }
    }
}
