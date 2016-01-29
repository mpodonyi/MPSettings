using System;
using System.IO;
using FluentAssertions;
using MPSettings.Test.TestData.GetSettingsWithContext;
using Xunit;

namespace MPSettings.Test.UnitTests
{
	public class GetSettingsWithContextTests : TestBase
	{
		public GetSettingsWithContextTests()
		{
			ConfigureProvider(@"TestData\GetSettingsWithContext\settings.config");
		}



	

		[Fact]
		public void Settings_GetPOCO_Test()
		{
			var setman = SettingsManager.Create<TestContext1>();

			TestSetting set = setman.GetSettings<TestSetting>(new TestContext1 { UserId = 1 });

			set.Foo.Should().Be(6);
			set.Bar.Should().Be("Mike1");
			set.InnerTest.InnerFoo.Should().Be("Mike3");


			TestSetting set2 = setman.GetSettings<TestSetting>(new TestContext1 { UserId = 2 });

			set2.Foo.Should().Be(6);
			set2.Bar.Should().Be("Mike");
			set2.InnerTest.InnerFoo.Should().Be("Mike4");

		}

		[Fact]
		public void Settings_GetDynamic_Test()
		{
			dynamic set = SettingsManager.Create<TestContext1>().GetSettingsDynamic(new TestContext1 {UserId = 1});

			((int)set.Foo).Should().Be(6);
			((string)set.Bar).Should().Be("Mike1");
			((string)set.InnerTest.InnerFoo).Should().Be("Mike3");

		}

	}
}