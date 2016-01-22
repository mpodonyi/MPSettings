using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSettings.Test
{
	public abstract class TestBase
	{
		protected void ConfigureProvider(string path)
		{
			object fs=null;

			if (SettingsProviders.InitValues.TryGetValue("dataStream", out fs) && fs != null)
			{
				IDisposable disposable = fs as IDisposable;
				if (disposable != null)
				{
					try
					{
						disposable.Dispose();
					}
					catch
					{
						// ignored
					}
				}
			}

			SettingsProviders.InitValues.Clear();

			FileStream obj = File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
			SettingsProviders.InitValues.Add("dataStream", obj);
			SettingsProviders.Providers.Reload();
		}


	}
}
