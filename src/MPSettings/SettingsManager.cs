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


        private IEnumerable<SettingsProperty> GetSettingsProperties(object baseObject, Type type, SettingsPropertyName path, PropertyInfo basePropInfo, SettingsContext context)
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
                        yield return new SettingsProperty(path + tuple.Item1, tuple.Item2, new SettingsContext(context))
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

        public dynamic GetSettingsDynamic(object context)
        {
            return new DynamicSettingsObject(SetRepo, SettingsPropertyName.Root, ToSettingsContext(context));
        }


        public T GetSettings<T>() where T : new()
        {
            return GetSettings<T>(null);
        }

        public T GetSettings<T>(object context) where T : new()
        {

            foreach (var settingsPropertyValue in SetRepo.GetPropertyValues(GetSettingsProperties(null, typeof(T), SettingsPropertyName.Root, null, ToSettingsContext(context))))
            {
                PropertyInfo SettAccessor = settingsPropertyValue.SettingsProperty.InternalContext["__SettAccessor"] as PropertyInfo;

                object obj = ObjDict.Get(settingsPropertyValue.SettingsProperty.PropertyName.Path);

                SettAccessor.SetValue(obj, settingsPropertyValue.PropertyValue);
            }

            return (T)ObjDict.Get(SettingsPropertyName.Root);
        }


        private SettingsContext ToSettingsContext(object obj)
        {
            string stringObj = obj as string;


            if (stringObj != null)
                return new SettingsContext()
                {
                    {"key", stringObj}
                };



            return null;



        }

    }


}
