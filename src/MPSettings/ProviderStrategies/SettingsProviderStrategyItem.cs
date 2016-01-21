using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MPSettings.Provider;

namespace MPSettings.ProviderStrategies
{
	public class SettingsProviderStrategyItem<TSettContext>
	{
		internal readonly ISettingsProvider SettProvider;
		private readonly Dictionary<string, Func<TSettContext, object>> _PropNames = new Dictionary<string, Func<TSettContext, object>>();


		private static IEnumerable<Tuple<string, Type, PropertyInfo>> GetNameAndType(Type type)
		{
			return from i in type.GetTypeInfo().DeclaredProperties
				   where i.GetMethod.IsPublic
				   select new Tuple<string, Type, PropertyInfo>(i.Name, i.PropertyType, i);
		}

		public SettingsProviderStrategyItem(ISettingsProvider settProvider, params Expression<Func<TSettContext, object>>[] expression)
		{
			SettProvider = settProvider;
			foreach (var expr in expression)
				_PropNames.Add(expr.Name, expr.Compile());
		}

		public SettingsProviderStrategyItem(ISettingsProvider settProvider, Type type)
		{
			SettProvider = settProvider;
			foreach (var tuple in GetNameAndType(type))
			{
				ParameterExpression parameter = Expression.Parameter(typeof(TSettContext));
				MemberExpression property = Expression.Property(parameter, tuple.Item3);
				UnaryExpression convert = Expression.Convert(property, typeof(object));
				var retval = Expression.Lambda<Func<TSettContext, object>>(convert, parameter);
				_PropNames.Add(tuple.Item1, retval.Compile());
			}
		}


		//private object GetDefaultValue(Type t)
		//{
		//	if (t.GetTypeInfo().IsValueType)
		//		return Activator.CreateInstance(t);

		//	return null;
		//}


		internal IDictionary<string, string> GetKeyValue(TSettContext obj)
		{
			Dictionary<string, string> retval = new Dictionary<string, string>();

			//MP: build settings context
			foreach (KeyValuePair<string, Func<TSettContext, object>> expr in _PropNames)
			{
				string name = expr.Key;
				//object defaultValue = GetDefaultValue(expr.ReturnType);
				object value = expr.Value(obj);

				retval.Add(name, value.ToString());
			}


			return retval;
		}
	}
}