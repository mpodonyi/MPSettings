using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MPSettings.Provider
{
    public interface ISettings
    {
        void Initialize(SettingsBridge repository);
    }

    public class SettingsBridge
    {
        internal SettingsBridge(SettingsProvider[] provider)
        { 
        
        }
    
    }





    
}
