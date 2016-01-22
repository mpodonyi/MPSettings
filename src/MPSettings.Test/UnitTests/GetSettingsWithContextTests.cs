using System;
using System.IO;
using FluentAssertions;
using MPSettings.Test.TestData.GetSettings;
using Xunit;

namespace MPSettings.Test.UnitTests
{
	public class GetSettingsWithContextTests : TestBase
	{
		public GetSettingsWithContextTests()
		{
			ConfigureProvider(@"TestData\GetSettings\settings.config");
		}


		public class MySett
		{
			public int UserId { get; set; }
		}

		public class MySett2
		{
			public int UserId { get; set; }
		}


		[Fact]
		public void Settings_GetPOCO_Test()
		{
			var setman = SettingsManager.Create<MySett>();

			TestSetting set = setman.GetSettings<TestSetting>(new MySett { UserId = 55 });

			set.Foo.Should().Be(6);
			set.Bar.Should().Be("Mike");
			set.InnerTest.InnerFoo.Should().Be("Mike2");
		}

	}
}