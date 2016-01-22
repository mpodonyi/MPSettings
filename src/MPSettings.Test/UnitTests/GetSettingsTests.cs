using System;
using Xunit;
using FluentAssertions;
using System.IO;
using System.Collections.Generic;
using MPSettings.Provider;
using MPSettings.Test.TestData.GetSettings;

namespace MPSettings.Test.UnitTests
{
	public class GetSettingsTests : TestBase
	{
		public GetSettingsTests()
		{
			ConfigureProvider(@"TestData\GetSettings\settings.config");
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
