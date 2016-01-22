using MPSettings.Test.TestData.GetSettings;

namespace MPSettings.Test.TestData.GetSettingsWithContext
{
	public class TestSetting
	{
		public int Foo { get; set; }

		public string Bar { get; set; }

		public InnerTestSetting InnerTest { get; set; }
	}
}