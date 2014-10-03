using System;
using Xunit;
using FluentAssertions;
using System.IO;
using System.Collections.Generic;

namespace MPSettings.Test.UnitTests
{
    public class TestSetting
    {
        public int Foo { get; set; }

        public string Bar { get; set; }
    }




    public class UnitTests
    {
        [Fact]
        public void TestMethodX()
        {
            string apppath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string combinedpath = Path.Combine(apppath, "settings.config");

            SettingsProviders.AppSettingsProvider.DefaultSettingsProvider.Initialize(new Dictionary<string, object> { { "path", combinedpath } });

            TestSetting set = SettingsManager.Instance.GetSettings<TestSetting>();

            set.Foo.Should().Be(6);
            set.Bar.Should().Be("Mike");


        }
    }
}
