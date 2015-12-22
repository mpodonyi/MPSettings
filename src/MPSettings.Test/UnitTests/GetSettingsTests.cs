using System;
using Xunit;
using FluentAssertions;
using System.IO;
using System.Collections.Generic;
using MPSettings.Provider;

namespace MPSettings.Test.UnitTests
{


    public class InnerTestSetting
    {
        public string InnerFoo { get; set; }
    }


    public class TestSetting
    {
        public int Foo { get; set; }

        public string Bar { get; set; }

        public InnerTestSetting InnerTest { get; set; }
    }




    public class GetSettingsTests
    {
        static GetSettingsTests()
        {
            FileStream obj = File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.config"));

            SettingsProviders.AddProviderInitValue("dataStream", obj);
        }





        [Fact]
        public void Settings_GetPOCO_Test()
        {
            TestSetting set = SettingsManager.Create().GetSettings<TestSetting>();

            set.Foo.Should().Be(6);
            set.Bar.Should().Be("Mike");
            set.InnerTest.InnerFoo.Should().Be("Mike2");
        }



        [Fact]
        public void Settings_GetDynamic_Test()
        {
            dynamic set = SettingsManager.Create().GetSettingsDynamic();

            ((int)set.Foo).Should().Be(6);
            ((string)set.Bar).Should().Be("Mike");
            ((string)set.InnerTest.InnerFoo).Should().Be("Mike2");


            Action act = () => { var uu = set.InnerWhatever; };
            act.ShouldThrow<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>();


        }
    }
}
