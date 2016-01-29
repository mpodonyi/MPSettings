using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MPSettings.Core;
using MPSettings.Test.TestData.XmlSettingsProviderTests;
using Xunit;

namespace MPSettings.Test.UnitTests
{
	public class XmlSettingsProviderTests : TestBase
	{
		public XmlSettingsProviderTests()
		{
			ConfigureProvider(@"TestData\XmlSettingsProviderTests\settings.config");
		}

		[Fact]
		public void Deserialize_Array_Test()
		{
			IEnumerable<string> aaa = new List<string>{"mike", "was", "here" };

			SettingsProperty setprop = new SettingsProperty("mike", aaa.GetType(), null);
			SettingsPropertyValue spv = new SettingsPropertyValue(setprop);

			spv.PropertyValue = aaa;

			string serializedval = spv.SerializedValue;










			TestSetting set = SettingsManager.Create().GetSettings<TestSetting>();

			set.Foo.Should().Be(6);
			set.Bar.Should().BeEquivalentTo("mike", "was", "here");
			set.InnerTest.InnerFoo.Should().Be("Mike2");
		}


	}
}
