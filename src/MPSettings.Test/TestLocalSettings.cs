
namespace MPSettings.Test.Properties
{

    
    internal sealed partial class Settings2 : global::System.Configuration.ApplicationSettingsBase
    {
        private static Settings2 defaultInstance = ((Settings2)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings2())));

        public static Settings2 Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [global::System.Configuration.SettingsProvider(typeof(FakeProvider))]
        [global::System.Configuration.ApplicationScopedSetting()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("test")]
        public string hyper
        {
            get
            {
                return ((string)(this["hyper"]));
            }
            set
            {
                this["hyper"] = value;
            }
        }

         

    }
}
