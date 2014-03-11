using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;


namespace MPSettings.Provider
{


#if ttt


    public class DotNetSettingsAdapter : SettingsBase, ISettingsAdapter
    {

        public DotNetSettingsAdapter(DotNetSettingsProviderAdapter settingsProvider, string groupname)
        {
            base.Initialize(new SettingsContext(), new SettingsPropertyCollection(), new SettingsProviderCollection());

            Context["GroupName"] = groupname;
            Providers.Add(settingsProvider);
            //Providers.Add()
            //SettingsProvider = settingsProvider;
        }


        //private readonly SettingsProvider SettingsProvider;

        //private SettingsPropertyCollection _settings;

        //public override SettingsPropertyCollection Properties
        //{
        //    get
        //    {
        //        if(this._settings == null)
        //        {
        //            if(!base.IsSynchronized)
        //            {
        //                this._settings = new SettingsPropertyCollection();
        //            }
        //            else
        //            {
        //                lock(this)
        //                {
        //                    if(this._settings == null)
        //                    {
        //                        this._settings = new SettingsPropertyCollection();
        //                    }
        //                }
        //            }
        //        }
        //        return this._settings;
        //    }
        //}

        //public override SettingsProviderCollection Providers
        //{
        //    get
        //    {
        //        if(this._providers == null)
        //        {
        //            if(!base.IsSynchronized)
        //            {
        //                this._providers = new SettingsProviderCollection();
        //                //this._providers.Add(new ProxyProvider());

        //            }
        //            else
        //            {
        //                lock(this)
        //                {
        //                    if(this._providers == null)
        //                    {
        //                        this._providers = new SettingsProviderCollection();
        //                    }
        //                }
        //            }
        //        }
        //        return this._providers;
        //    }
        //}


        //public override object this[string propertyName]
        //{
        //    get
        //    {
        //        return base[propertyName];
        //    }
        //    set
        //    {
        //        base[propertyName] = value;
        //    }
        //}

        public void RegisterProperty(string name, Type type)
        {
            if(Properties[name] == null)
            {
                var prop = new SettingsProperty(name, type, Providers["DotNetSettingsProviderAdapter"], false, null, SettingsSerializeAs.ProviderSpecific, null, true, true);

                Properties.Add(prop);
            }
        }

        //private SettingsProperty Initializer
        //{
        //    get
        //    {
        //        if(this._init == null)
        //        {
        //            this._init = new SettingsProperty("")
        //            {
        //                DefaultValue = null,
        //                IsReadOnly = false,
        //                PropertyType = null
        //            };
        //            SettingsProvider localFileSettingsProvider = new LocalFileSettingsProvider();
        //            if(this._classAttributes != null)
        //            {
        //                for(int i = 0; i < (int)this._classAttributes.Length; i++)
        //                {
        //                    Attribute attribute = this._classAttributes[i] as Attribute;
        //                    if(attribute != null)
        //                    {
        //                        if(attribute is ReadOnlyAttribute)
        //                        {
        //                            this._init.IsReadOnly = true;
        //                        }
        //                        else if(attribute is SettingsGroupNameAttribute)
        //                        {
        //                            if(this._context == null)
        //                            {
        //                                this._context = new SettingsContext();
        //                            }
        //                            this._context["GroupName"] = ((SettingsGroupNameAttribute)attribute).GroupName;
        //                        }
        //                        else if(attribute is SettingsProviderAttribute)
        //                        {
        //                            string providerTypeName = ((SettingsProviderAttribute)attribute).ProviderTypeName;
        //                            Type type = Type.GetType(providerTypeName);
        //                            if(type == null)
        //                            {
        //                                object[] objArray = new object[] { providerTypeName };
        //                                throw new ConfigurationErrorsException(SR.GetString("ProviderTypeLoadFailed", objArray));
        //                            }
        //                            SettingsProvider settingsProvider = SecurityUtils.SecureCreateInstance(type) as SettingsProvider;
        //                            if(settingsProvider == null)
        //                            {
        //                                object[] objArray1 = new object[] { providerTypeName };
        //                                throw new ConfigurationErrorsException(SR.GetString("ProviderInstantiationFailed", objArray1));
        //                            }
        //                            localFileSettingsProvider = settingsProvider;
        //                        }
        //                        else if(!(attribute is SettingsSerializeAsAttribute))
        //                        {
        //                            this._init.Attributes.Add(attribute.GetType(), attribute);
        //                        }
        //                        else
        //                        {
        //                            this._init.SerializeAs = ((SettingsSerializeAsAttribute)attribute).SerializeAs;
        //                            this._explicitSerializeOnClass = true;
        //                        }
        //                    }
        //                }
        //            }
        //            localFileSettingsProvider.Initialize(null, null);
        //            localFileSettingsProvider.ApplicationName = ConfigurationManagerInternalFactory.Instance.ExeProductName;
        //            this._init.Provider = localFileSettingsProvider;
        //        }
        //        return this._init;
        //    }
        //}
      
    }

#endif
}
