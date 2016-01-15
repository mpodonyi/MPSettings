using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MPSettings.Core;
using MPSettings.Provider;

namespace MPSettings.ProviderStrategies
{
    public sealed class SettingsProviderStrategyCollectionDictionary
    {
        public static readonly SettingsProviderStrategyCollectionDictionary Instance = new SettingsProviderStrategyCollectionDictionary();


        public  SettingsProviderStrategyCollectionBase<TSETT> GetSPSC<TSETT>()
        {
            if(typeof(TSETT) == typeof(UserSettingsContext))
                return new DefaultSettingsProviderStrategyCollection() as SettingsProviderStrategyCollectionBase<TSETT>;
            if (typeof(TSETT) == typeof(object))
                return new SimpleSettingsProviderStrategyCollection() as SettingsProviderStrategyCollectionBase<TSETT>;


            throw new NotImplementedException();
        }


    }


  


    //

    public class SettingsProviderStrategy<TSettContext>
    {
        public readonly ISettingsProvider SettProvider;
        public readonly Expression<Func<TSettContext, object>>[] Expressions;

        public SettingsProviderStrategy(ISettingsProvider settProvider, params Expression<Func<TSettContext, object>>[] expression)
        {
            SettProvider = settProvider;
            Expressions = expression;
        }
    }





    public abstract class SettingsProviderStrategyCollectionBase<TSettContext>
    {
        protected abstract IEnumerable<SettingsProviderStrategy<TSettContext>> GetSettingsProviderStrategy(IReadOnlyList<ISettingsProvider> spl, TSettContext sett);






        //private readonly IList<SettingsProviderStrategy<TSettContext>> SettingsProviderStrategy = new List<SettingsProviderStrategy<TSettContext>>();

        private IReadOnlyList<T> ToReadOnlyList<T>(IEnumerable<T> list)
        {
            return new ReadOnlyCollection<T>(list.ToList());
        }


        object GetDefaultValue(Type t)
        {
            if (t.GetTypeInfo().IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }


        internal IEnumerable<Tuple<ISettingsProvider, SettingsContext>> GetProviderAndContext(TSettContext obj,IEnumerable<ISettingsProvider> spl)
        {
            Dictionary<object,object> whatever=new Dictionary<object, object>();

            foreach (SettingsProviderStrategy<TSettContext> sPS in GetSettingsProviderStrategy(ToReadOnlyList(spl), obj))
            {
                SettingsContext retSettCtx = null;

                //MP: build settings context
                foreach (Expression<Func<TSettContext, object>> expr in sPS.Expressions)
                {
                    string name = expr.Name;
                    object defaultValue = GetDefaultValue(expr.ReturnType);
                    object value = expr.Compile().Invoke(obj);


                }

                yield return Tuple.Create<ISettingsProvider, SettingsContext>(sPS.SettProvider, retSettCtx);

            }
        }






        //protected void Add(ISettingsProvider settProvider, params Expression<Func<TSettContext, object>>[] expression)
        //{
        //    SettingsProviderStrategy.Add(new SettingsProviderStrategy<TSettContext>(settProvider, expression));
        //}

        //protected void Add(ISettingsProvider settProvider)
        //{
        //    SettingsProviderStrategy.Add(new SettingsProviderStrategy<TSettContext>(settProvider, null));
        //}


    }


    public class DefaultSettingsProviderStrategyCollection : SettingsProviderStrategyCollectionBase<UserSettingsContext>
    {
        protected override IEnumerable<SettingsProviderStrategy<UserSettingsContext>> GetSettingsProviderStrategy(IReadOnlyList<ISettingsProvider> spl, UserSettingsContext obj)
        {
            foreach (ISettingsProvider settProvider in spl)
            {
                yield return new SettingsProviderStrategy<UserSettingsContext>(settProvider, sett => sett.UserId, sett => sett.Domain);
                yield return new SettingsProviderStrategy<UserSettingsContext>(settProvider, sett => sett.Domain);
                yield return new SettingsProviderStrategy<UserSettingsContext>(settProvider);
            }
        }
    }

    public class SimpleSettingsProviderStrategyCollection : SettingsProviderStrategyCollectionBase<object>
    {
        protected override IEnumerable<SettingsProviderStrategy<object>> GetSettingsProviderStrategy(IReadOnlyList<ISettingsProvider> spl, object obj)
        {
            foreach (ISettingsProvider settProvider in spl)
            {
                yield return new SettingsProviderStrategy<object>(settProvider);
            }
        }
    }

    

    





    public class UserSettingsContext
    {
        public string UserId { get; set; }
        public int Domain { get; set; }


    }




}
