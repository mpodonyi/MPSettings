using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using MPSettings.Provider;
using MPSettings.Core;
using System.Reflection;
using MPSettings.Dynamic;
using MPSettings.Utils;

namespace MPSettings
{
    public static  class SettingsManager
    {
        public static ISettBasic Create()
        {
            SettingsProviderList spl = SettingsProviders.Providers.Count == 0
                ? new SettingsProviderList {SettingsProviders.Providers.DefaultProvider}
                : SettingsProviders.Providers;


            SettingsProviderStrategyCollectionDictionary spsc = SettingsProviderStrategyCollectionDictionary.Instance;

            return new SettImpl<object>(spl,spsc);
        }

        public static ISettWithContext<TSETT> Create<TSETT>() where TSETT : class
        {
            SettingsProviderList spl = SettingsProviders.Providers.Count == 0
               ? new SettingsProviderList { SettingsProviders.Providers.DefaultProvider }
               : SettingsProviders.Providers;


            SettingsProviderStrategyCollectionDictionary spsc = SettingsProviderStrategyCollectionDictionary.Instance;

            return new SettImpl<TSETT>(spl,spsc);
        }


    }

    public interface ISettBasic
    {
        dynamic GetSettingsDynamic();
        T GetSettings<T>() where T : new();
    }

    public interface ISettWithContext<in TSETT> : ISettBasic
        where TSETT :class
    {
        dynamic GetSettingsDynamic(TSETT context);
        T GetSettings<T>(TSETT context) where T : new();

    }


    internal class SettImpl<TSETT> : ISettWithContext<TSETT> where TSETT : class
    {
        //private static readonly Lazy<SettImpl<TSETT>> lazy = new Lazy<SettImpl<TSETT>>(() => new SettImpl<TSETT>());

        //public static SettImpl<TSETT> Instance { get { return lazy.Value; } }

        private readonly SettingsRepository SetRepo;
        private readonly ObjectDictionary ObjDict;

        internal SettImpl(SettingsProviderList spl, SettingsProviderStrategyCollectionDictionary spsc)
        {
            ObjDict = new ObjectDictionary();

            SetRepo = new SettingsRepository(spl, spsc);
        }


        private IEnumerable<SettingsProperty> GetSettingsProperties(object baseObject, Type type, SettingsPropertyName path, PropertyInfo basePropInfo)
        {
            if (Reflector.HasParameterLessDefaultConstructor(type))
            {
                object obj = Activator.CreateInstance(type);
                ObjDict.Add(path, obj);

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
                        foreach (var settingsProperty in GetSettingsProperties(obj, tuple.Item2, path + tuple.Item1, tuple.Item3))
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

            foreach (var settingsPropertyValue in SetRepo.GetPropertyValues(context, GetSettingsProperties(null, typeof(T), SettingsPropertyName.Root, null)))
            {
                PropertyInfo SettAccessor = settingsPropertyValue.SettingsProperty.InternalContext["__SettAccessor"] as PropertyInfo;

                object obj = ObjDict.Get(settingsPropertyValue.SettingsProperty.PropertyName.Path);

                SettAccessor.SetValue(obj, settingsPropertyValue.PropertyValue);
            }

            return (T)ObjDict.Get(SettingsPropertyName.Root);
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
