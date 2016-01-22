using System;
using System.Collections.Generic;
using System.Reflection;
using MPSettings.Core;
using MPSettings.Dynamic;
using MPSettings.ProviderStrategies;
using MPSettings.Utils;

namespace MPSettings.Internal
{
	internal class SettImpl<TSETT> : ISettWithContext<TSETT> where TSETT : class
	{
		//private static readonly Lazy<SettImpl<TSETT>> lazy = new Lazy<SettImpl<TSETT>>(() => new SettImpl<TSETT>());

		//public static SettImpl<TSETT> Instance { get { return lazy.Value; } }

		private readonly SettingsRepository SetRepo;
		private readonly ObjectDictionary ObjDict;

		internal SettImpl(SettingsProviderManager spl, SettingsProviderStrategyCollection spsc)
		{
			ObjDict = new ObjectDictionary();

			SetRepo = new SettingsRepository(spl, spsc);
		}


		private IEnumerable<SettingsProperty> GetSettingsProperties(object baseObject, Type type, SettingsPropertyName path, PropertyInfo basePropInfo, object context)
		{
			if (Reflector.HasParameterLessDefaultConstructor(type))
			{
				object obj = Activator.CreateInstance(type);
				ObjDict.Add(path, context, obj);

				if (baseObject != null)
				{
					basePropInfo.SetValue(baseObject, obj);
				}

				foreach (var tuple in Reflector.GetNameAndType(type))
				{
					if (Reflector.IsSimpleType(tuple.Item2))
					{
						yield return new SettingsProperty(path + tuple.Item1, tuple.Item2, null)
						{
							InternalContext = new Dictionary<string, object>
							{
								{"__SettAccessor", tuple.Item3},
							}
						};
					}
					else
					{
						foreach (var settingsProperty in GetSettingsProperties(obj, tuple.Item2, path + tuple.Item1, tuple.Item3, context))
						{
							yield return settingsProperty;
						}

					}
				}

			}

		}


		public dynamic GetSettingsDynamic()
		{
			return GetSettingsDynamic(null);
		}

		public dynamic GetSettingsDynamic(TSETT context)
		{
			return new DynamicSettingsObject<TSETT>(SetRepo, SettingsPropertyName.Root, context);
		}




		public T GetSettings<T>() where T : new()
		{
			return GetSettings<T>(null);
		}

		public T GetSettings<T>(TSETT context) where T : new()
		{

			foreach (var settingsPropertyValue in SetRepo.GetPropertyValues(context, GetSettingsProperties(null, typeof(T), SettingsPropertyName.Root, null, context)))
			{
				PropertyInfo SettAccessor = settingsPropertyValue.SettingsProperty.InternalContext["__SettAccessor"] as PropertyInfo;

				object obj = ObjDict.Get(settingsPropertyValue.SettingsProperty.PropertyName.Path, context);

				SettAccessor.SetValue(obj, settingsPropertyValue.PropertyValue);
			}

			return (T)ObjDict.Get(SettingsPropertyName.Root, context);
		}


		//private SettingsContext ToSettingsContext(object obj)
		//{
		//    string stringObj = obj as string;


		//    if (stringObj != null)
		//        return new SettingsContext()
		//        {
		//            {"key", stringObj}
		//        };



		//    return null;



		//}

	}
}