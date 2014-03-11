using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSettings;



namespace MPSettings.Test.UnitTests
{


    [TestClass]
    public class FakeProviderTest
    {
        private static readonly SettingsManager settingsManager = SettingsManager.GetSettingsManager().AddSettingsProvider(new FakeProvider());

        private static readonly StaticTestSettings Sett = settingsManager.GetSettings<StaticTestSettings>();




        [TestMethod]
        public void TestMethod1()
        {

            
            //SettingsManager.AddSettingsProvider();


        }
    }
}
