using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings.Core
{
    public abstract class SettingBase: ISetting
    {
        private SettingsFactory bridge;
      

        protected T Get<T>(string name)
        {
            return default(T);
        }

        protected void Set<T>(string name, T value)
        {

        }


       



        #region ISettings Members

        void ISetting.Initialize(SettingsFactory repository)
        {
            bridge=repository;
        }

        #endregion
    }
}
