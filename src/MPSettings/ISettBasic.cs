namespace MPSettings
{
	public interface ISettBasic
	{
		dynamic GetSettingsDynamic();
		T GetSettings<T>() where T : new();
	}
}