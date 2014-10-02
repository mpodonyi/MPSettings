using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPSettings.Provider;
using MPSettings.Core;

namespace MPSettings.Test
{
    public class StaticTestSettings: SettingBase
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


    public class NestedStaticTestSettings : SettingBase
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
