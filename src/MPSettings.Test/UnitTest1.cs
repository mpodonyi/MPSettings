using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPSettings.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            dynamic globalSettings = SettingsManager.GetSettings();

            int foo = globalSettings.Mike.Bar;
            Assert.AreEqual(7, foo);

        }
    }
}
