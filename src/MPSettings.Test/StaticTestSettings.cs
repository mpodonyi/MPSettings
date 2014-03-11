using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPSettings.Provider;

namespace MPSettings.Test
{
    public class StaticTestSettings: SettingsBase
    {
        public bool Mike
        {
            get
            {
                return Get<bool>("Mike");
            
            }
            set
            {
                Set("Mike", value);
            }
        }

        public NestedStaticTestSettings NestedMike
        {
            get
            {
                return Get<NestedStaticTestSettings>("NestedMike");

            }
            set
            {
                Set("NestedMike", value);
            }
        }





    }


    public class NestedStaticTestSettings : SettingsBase
    {
        public bool Mike
        {
            get
            {
                return Get<bool>("Mike");

            }
            set
            {
                Set("Mike", value);
            }
        }


    }




}
