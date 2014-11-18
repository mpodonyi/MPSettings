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
            TestSetting set = SettingsManager.Instance.GetSettings<TestSetting>();

            set.Foo.Should().Be(6);
            set.Bar.Should().Be("Mike");
        }
    }
}
