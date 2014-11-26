using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using MPSettings.Provider;
using MPSettings.Core;
using MPSettings.Reflection;
using System.Reflection;

namespace MPSettings
{
    public sealed class SettingsManager
    {
        private static readonly Lazy<SettingsManager> lazy = new Lazy<SettingsManager>(() => new SettingsManager());

        public static SettingsManager Instance { get { return lazy.Value; } }

        private readonly SettingsRepository SetRepo;

        private SettingsManager()
        {
            SetRepo = new SettingsRepository(SettingsProviders.AppSettingsProvider);
        }



        public T GetSettings<T>() where T : new()
        {
            //ISettingsAdapter adap = new DotNetSettingsAdapter(new DotNetSettingsProviderAdapter(_SettingsProviders), "");

            var obj = new T();
            ISetting settings = obj as ISetting;
            if (settings != null)
            {
                settings.Initialize(SetRepo);
            }
            else
            {
                List<SettingsProperty> settproplist = new List<SettingsProperty>();

                NewMethod(obj, settproplist);

                foreach (var propValue in SetRepo.GetPropertyValues(settproplist))
                {
                    Reflection.Reflector.SetProperty(
                        propValue.SettingsProperty.Context["_propobject"],
                        propValue.SettingsProperty.Context["_propinfo"] as PropertyInfo,
                        propValue.PropertyValue);
                }
            }

            return obj;
        }

        private static void NewMethod(object obj, List<SettingsProperty> settproplist, string pre) 
        {
            foreach (var propinfo in Reflection.Reflector.GetProperties(obj))
            {
                if (Reflection.Reflector.IsSimpleType(propinfo))
                {
                    settproplist.Add(SettingsPropertyCreateFrom(propinfo, obj, pre));
                }
                else
                {
                    Type declType = propinfo.PropertyType;
                    object obj2 = Activator.CreateInstance(declType);
                    propinfo.SetValue(obj, obj2, null);

                    NewMethod(obj2, settproplist, pre + propinfo.Name);



                }
            }
        }

        private static SettingsProperty SettingsPropertyCreateFrom(PropertyInfo propertyInfo, object propertyObject, string pre)
        {
            return new SettingsProperty(pre+"."+propertyInfo.Name, propertyInfo.PropertyType, new Dictionary<string, object> { { "_propinfo", propertyInfo }, { "_propobject", propertyObject } });
        }


    }


}
