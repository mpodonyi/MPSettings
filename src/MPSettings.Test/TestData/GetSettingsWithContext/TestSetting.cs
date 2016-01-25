namespace MPSettings.Test.TestData.GetSettingsWithContext
{
	public class TestSetting
	{
		public int Foo { get; set; }

		public string Bar { get; set; }

		public InnerTestSetting InnerTest { get; set; }
	}


	public class InnerTestSetting
	{
		public string InnerFoo { get; set; }
	}
}