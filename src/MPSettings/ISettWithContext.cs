using System.Dynamic;
using System.Linq;
using System.Text;
using MPSettings.Provider;

namespace MPSettings
{
	public interface ISettWithContext<in TSETT> : ISettBasic
        where TSETT :class
    {
        dynamic GetSettingsDynamic(TSETT context);
        T GetSettings<T>(TSETT context) where T : new();

    }
}
