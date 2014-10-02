using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace YamlSettingsProvider
{
    public static class SettingsManager
    {
        private static ConcurrentDictionary<Type, ApplicationSettingsBase> InstanceHolder = new ConcurrentDictionary<Type, ApplicationSettingsBase>();

        public static T GetSettings<T>() where T : ApplicationSettingsBase, new()
        {
            ApplicationSettingsBase retval = null;
            if(InstanceHolder.TryGetValue(typeof(T), out retval))
            {
                return (T)retval;
            }

            lock(InstanceHolder)
            {
                if(InstanceHolder.TryGetValue(typeof(T), out retval))
                {
                    return (T)retval;
                }

                InstanceHolder[typeof(T)] = ((T)(ApplicationSettingsBase.Synchronized(new T())));
                return (T)InstanceHolder[typeof(T)];
            }

        }
    }
}
