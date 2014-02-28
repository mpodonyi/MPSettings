using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using MPSettings.YamlProvider;

namespace MPSettings.Test
{
    public class Meier: DynamicSettings
    {
        public bool Mike { get; set; }


        public string Mike1 { get; set; }
    }


    [TestClass]
    public class SettingsTest
    {
        [TestMethod]
        public void GetSettingsDynamic_Success()
        {
            SettingsManager.AddSettingsProvider(new YamlSettingsProvider());

            dynamic sett = SettingsManager.GetSettings();

            sett.FireStarter.Should().Be("mike");

        }

        [TestMethod]
        public void GetSettingsStatic_Success()
        {
            var yy = MPSettings.Test.Properties.Settings2.Default.hyper;

            yy.Should().Be("test");
        }
    }


   
}
