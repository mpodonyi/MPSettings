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


    public sealed class SettingsManager
    {
        private static readonly Lazy<SettingsManager> lazy = new Lazy<SettingsManager>(() => new SettingsManager());

        public static SettingsManager Instance { get { return lazy.Value; } }

        private readonly SettingsRepository SetRepo;
        private readonly ObjectDictionary ObjDict;

        private SettingsManager()
        {
            ObjDict=new ObjectDictionary();
            SetRepo = new SettingsRepository(SettingsProviders.Providers.Count == 0
                ? new SettingsProviderList {SettingsProviders.Providers.DefaultProvider}
                : SettingsProviders.Providers);
        }


        private IEnumerable<SettingsProperty> GetSettingsProperties(object baseObject, Type type, SettingsPropertyName path, PropertyInfo basePropInfo, object context)
        {
            if (Reflector.HasParameterLessDefaultConstructor(type))
            {
                object obj = Activator.CreateInstance(type);
                ObjDict.Add(path, obj);

                if (baseObject != null)
                {
                    basePropInfo.SetValue(baseObject,obj);

                }

                foreach (var tuple in Reflector.GetNameAndType(type))
                {
                    if (Reflector.IsSimpleType(tuple.Item2))
                    {
                        yield return new SettingsProperty(path + tuple.Item1, tuple.Item2)
                        {
                            Context = new Dictionary<string, object>
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
            return new DynamicSettingsObject(SetRepo, "TestSetting");
        }


        public T GetSettings<T>() where T : new()
        {
            return GetSettings<T>(null);
        }

        public T GetSettings<T>(object context) where T : new()
        {
            string name = typeof (T).Name;


            foreach (var settingsPropertyValue in SetRepo.GetPropertyValues(GetSettingsProperties(null, typeof(T), name, null)))
            {
                PropertyInfo SettAccessor = settingsPropertyValue.SettingsProperty.Context["__SettAccessor"] as PropertyInfo;

                object obj = ObjDict.Get(settingsPropertyValue.SettingsProperty.PropertyName.Path);

                SettAccessor.SetValue(obj, settingsPropertyValue.PropertyValue);
            }

            return (T)ObjDict.Get(name);

            //if (typeof(ISetting).IsAssignableFrom(typeof(T)))
            //{
            //    T retval = new T();
            //    ISetting settings = retval as ISetting;

            //    settings.Initialize(SetRepo);

            //    return retval;
            //}

        }



    }


}
