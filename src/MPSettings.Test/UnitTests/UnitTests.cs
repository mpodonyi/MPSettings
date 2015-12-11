using System;
using Xunit;
using FluentAssertions;
using System.IO;
using System.Collections.Generic;

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




    public class UnitTests
    {
        private Stream GetStream(string fileName)
        {
            return File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));
        }




        [Fact]
        public void TestMethodX()
        {
            SettingsProviders.AddProviderInitValue("dataStream", GetStream("settings.config"));
            
            TestSetting set = SettingsManager.Instance.GetSettings<TestSetting>();

            set.Foo.Should().Be(6);
            set.Bar.Should().Be("Mike");
            set.InnerTest.InnerFoo.Should().Be("Mike2");
        }
    }
}
