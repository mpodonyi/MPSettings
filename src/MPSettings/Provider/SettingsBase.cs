using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings.Provider
{
    public abstract class SettingsBase: ISettings
    {
        private SettingsBridge bridge;
      

        protected T Get<T>(string name)
        {
            return default(T);
        }

        protected void Set<T>(string name, T value)
        {

        }


       



        #region ISettings Members

        void ISettings.Initialize(SettingsBridge repository)
        {
            bridge=repository;
        }

        #endregion
    }
}
