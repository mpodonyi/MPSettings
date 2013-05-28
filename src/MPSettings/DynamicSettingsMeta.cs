﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MPSettings
{
    partial class OLDDynamicSettings
    {
        private sealed class DynamicSettingsMeta : DynamicMetaObject
        {

            internal DynamicSettingsMeta(Expression expression, DynamicSettings value)
                : base(expression, BindingRestrictions.Empty, value)
            {
            }

            public override System.Collections.Generic.IEnumerable<string> GetDynamicMemberNames()
            {
                return Value.GetDynamicMemberNames();
            }

            public override DynamicMetaObject BindGetMember(GetMemberBinder binder)
            {
                if(IsOverridden("TryGetMember"))
                {
                    return CallMethodWithResult("TryGetMember", binder, NoArgs, (e) => binder.FallbackGetMember(this, e));
                }

                return base.BindGetMember(binder);
            }

            public override DynamicMetaObject BindSetMember(SetMemberBinder binder, DynamicMetaObject value)
            {
                if(IsOverridden("TrySetMember"))
                {
                    return CallMethodReturnLast("TrySetMember", binder, NoArgs, value.Expression, (e) => binder.FallbackSetMember(this, value, e));
                }

                return base.BindSetMember(binder, value);
            }

            public override DynamicMetaObject BindDeleteMember(DeleteMemberBinder binder)
            {
                if(IsOverridden("TryDeleteMember"))
                {
                    return CallMethodNoResult("TryDeleteMember", binder, NoArgs, (e) => binder.FallbackDeleteMember(this, e));
                }

                return base.BindDeleteMember(binder);
            }

            public override DynamicMetaObject BindConvert(ConvertBinder binder)
            {
                if(IsOverridden("TryConvert"))
                {
                    return CallMethodWithResult("TryConvert", binder, NoArgs, (e) => binder.FallbackConvert(this, e));
                }

                return base.BindConvert(binder);
            }

            public override DynamicMetaObject BindInvokeMember(InvokeMemberBinder binder, DynamicMetaObject[] args)
            {
                // Generate a tree like:
                //
                // {
                //   object result; 
                //   TryInvokeMember(payload, out result)
                //      ? result 
                //      : TryGetMember(payload, out result) 
                //          ? FallbackInvoke(result)
                //          : fallbackResult 
                // }
                //
                // Then it calls FallbackInvokeMember with this tree as the
                // "error", giving the language the option of using this 
                // tree or doing .NET binding.
                // 
                Fallback fallback = e => binder.FallbackInvokeMember(this, args, e);

                var call = BuildCallMethodWithResult(
                    "TryInvokeMember",
                    binder,
                    GetExpressions(args),
                    BuildCallMethodWithResult(
                        "TryGetMember",
                        new GetBinderAdapter(binder),
                        NoArgs,
                        fallback(null),
                        (e) => binder.FallbackInvoke(e, args, null)
                    ),
                    null
                );

                return fallback(call);
            }


            public override DynamicMetaObject BindCreateInstance(CreateInstanceBinder binder, DynamicMetaObject[] args)
            {
                if(IsOverridden("TryCreateInstance"))
                {
                    return CallMethodWithResult("TryCreateInstance", binder, GetExpressions(args), (e) => binder.FallbackCreateInstance(this, args, e));
                }

                return base.BindCreateInstance(binder, args);
            }

            public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
            {
                if(IsOverridden("TryInvoke"))
                {
                    return CallMethodWithResult("TryInvoke", binder, GetExpressions(args), (e) => binder.FallbackInvoke(this, args, e));
                }

                return base.BindInvoke(binder, args);
            }

            public override DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject arg)
            {
                if(IsOverridden("TryBinaryOperation"))
                {
                    return CallMethodWithResult("TryBinaryOperation", binder, GetExpressions(new DynamicMetaObject[] { arg }), (e) => binder.FallbackBinaryOperation(this, arg, e));
                }

                return base.BindBinaryOperation(binder, arg);
            }

            internal static Expression[] GetExpressions(DynamicMetaObject[] objects)
            {
                //ContractUtils.RequiresNotNull(objects, "objects");

                Expression[] res = new Expression[objects.Length];
                for(int i = 0; i < objects.Length; i++)
                {
                    DynamicMetaObject mo = objects[i];
                    //ContractUtils.RequiresNotNull(mo, "objects");
                    Expression expr = mo.Expression;
                    //ContractUtils.RequiresNotNull(expr, "objects");
                    res[i] = expr;
                }

                return res;
            }

            public override DynamicMetaObject BindUnaryOperation(UnaryOperationBinder binder)
            {
                if(IsOverridden("TryUnaryOperation"))
                {
                    return CallMethodWithResult("TryUnaryOperation", binder, NoArgs, (e) => binder.FallbackUnaryOperation(this, e));
                }

                return base.BindUnaryOperation(binder);
            }

            public override DynamicMetaObject BindGetIndex(GetIndexBinder binder, DynamicMetaObject[] indexes)
            {
                if(IsOverridden("TryGetIndex"))
                {
                    return CallMethodWithResult("TryGetIndex", binder, GetExpressions(indexes), (e) => binder.FallbackGetIndex(this, indexes, e));
                }

                return base.BindGetIndex(binder, indexes);
            }

            public override DynamicMetaObject BindSetIndex(SetIndexBinder binder, DynamicMetaObject[] indexes, DynamicMetaObject value)
            {
                if(IsOverridden("TrySetIndex"))
                {
                    return CallMethodReturnLast("TrySetIndex", binder, GetExpressions(indexes), value.Expression, (e) => binder.FallbackSetIndex(this, indexes, value, e));
                }

                return base.BindSetIndex(binder, indexes, value);
            }

            public override DynamicMetaObject BindDeleteIndex(DeleteIndexBinder binder, DynamicMetaObject[] indexes)
            {
                if(IsOverridden("TryDeleteIndex"))
                {
                    return CallMethodNoResult("TryDeleteIndex", binder, GetExpressions(indexes), (e) => binder.FallbackDeleteIndex(this, indexes, e));
                }

                return base.BindDeleteIndex(binder, indexes);
            }

            private delegate DynamicMetaObject Fallback(DynamicMetaObject errorSuggestion);

            private readonly static Expression[] NoArgs = new Expression[0];

            private static Expression[] GetConvertedArgs(params Expression[] args)
            {
                ReadOnlyCollectionBuilder<Expression> paramArgs = new ReadOnlyCollectionBuilder<Expression>(args.Length);

                for(int i = 0; i < args.Length; i++)
                {
                    paramArgs.Add(Expression.Convert(args[i], typeof(object)));
                }

                return paramArgs.ToArray();
            }

            /// <summary> 
            /// Helper method for generating expressions that assign byRef call
            /// parameters back to their original variables 
            /// </summary>
            private static Expression ReferenceArgAssign(Expression callArgs, Expression[] args)
            {
                ReadOnlyCollectionBuilder<Expression> block = null;

                for(int i = 0; i < args.Length; i++)
                {
                    //System.Diagnostics.Contracts.Requires(args[i] is ParameterExpression);
                    if(((ParameterExpression)args[i]).IsByRef)
                    {
                        if(block == null)
                            block = new ReadOnlyCollectionBuilder<Expression>();

                        block.Add(
                            Expression.Assign(
                                args[i],
                                Expression.Convert(
                                    Expression.ArrayIndex(
                                        callArgs,
                                        Expression.Constant(i)
                                    ),
                                    args[i].Type
                                )
                            )
                        );
                    }
                }

                if(block != null)
                    return Expression.Block(block);
                else
                    return Expression.Empty();
            }

            /// <summary>
            /// Helper method for generating arguments for calling methods 
            /// on DynamicSettings.  parameters is either a list of ParameterExpressions 
            /// to be passed to the method as an object[], or NoArgs to signify that
            /// the target method takes no object[] parameter. 
            /// </summary>
            private static Expression[] BuildCallArgs(DynamicMetaObjectBinder binder, Expression[] parameters, Expression arg0, Expression arg1)
            {
                if(!object.ReferenceEquals(parameters, NoArgs))
                    return arg1 != null ? new Expression[] { Constant(binder), arg0, arg1 } : new Expression[] { Constant(binder), arg0 };
                else
                    return arg1 != null ? new Expression[] { Constant(binder), arg1 } : new Expression[] { Constant(binder) };
            }

            private static ConstantExpression Constant(DynamicMetaObjectBinder binder)
            {
                Type t = binder.GetType();
                while(!t.IsVisible)
                {
                    t = t.BaseType;
                }
                return Expression.Constant(binder, t);
            }

            /// <summary>
            /// Helper method for generating a MetaObject which calls a 
            /// specific method on Dynamic that returns a result
            /// </summary>
            private DynamicMetaObject CallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, Fallback fallback)
            {
                return CallMethodWithResult(methodName, binder, args, fallback, null);
            }

            /// <summary> 
            /// Helper method for generating a MetaObject which calls a
            /// specific method on Dynamic that returns a result 
            /// </summary>
            private DynamicMetaObject CallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, Fallback fallback, Fallback fallbackInvoke)
            {
                //
                // First, call fallback to do default binding 
                // This produces either an error or a call to a .NET member
                // 
                DynamicMetaObject fallbackResult = fallback(null);

                var callDynamic = BuildCallMethodWithResult(methodName, binder, args, fallbackResult, fallbackInvoke);

                //
                // Now, call fallback again using our new MO as the error
                // When we do this, one of two things can happen: 
                //   1. Binding will succeed, and it will ignore our call to
                //      the dynamic method, OR 
                //   2. Binding will fail, and it will use the MO we created 
                //      above.
                // 
                return fallback(callDynamic);
            }

            /// <summary> 
            /// Helper method for generating a MetaObject which calls a
            /// specific method on DynamicSettings that returns a result. 
            /// 
            /// args is either an array of arguments to be passed
            /// to the method as an object[] or NoArgs to signify that 
            /// the target method takes no parameters.
            /// </summary>
            private DynamicMetaObject BuildCallMethodWithResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, DynamicMetaObject fallbackResult, Fallback fallbackInvoke)
            {
                if(!IsOverridden(methodName))
                {
                    return fallbackResult;
                }

                //
                // Build a new expression like: 
                // {
                //   object result;
                //   TryGetMember(payload, out result) ? fallbackInvoke(result) : fallbackResult
                // } 
                //
                var result = Expression.Parameter(typeof(object), null);
                ParameterExpression callArgs = methodName != "TryBinaryOperation" ? Expression.Parameter(typeof(object[]), null) : Expression.Parameter(typeof(object), null);
                var callArgsValue = GetConvertedArgs(args);

                var resultMO = new DynamicMetaObject(result, BindingRestrictions.Empty);

                // Need to add a conversion if calling TryConvert
                if(binder.ReturnType != typeof(object))
                {
                    Debug.Assert(binder is ConvertBinder && fallbackInvoke == null);

                    var convert = Expression.Convert(resultMO.Expression, binder.ReturnType);
                    // will always be a cast or unbox
                    Debug.Assert(convert.Method == null);

#if !SILVERLIGHT
                    // Prepare a good exception message in case the convert will fail
                    string convertFailed = "error";


                    //Strings.DynamicSettingsResultNotAssignable(
                    //    "{0}",
                    //    this.Value.GetType(),
                    //    binder.GetType(),
                    //    binder.ReturnType
                    //);

                    Expression condition;
                    // If the return type can not be assigned null then just check for type assignablity otherwise allow null.
                    if(binder.ReturnType.IsValueType && Nullable.GetUnderlyingType(binder.ReturnType) == null)
                    {
                        condition = Expression.TypeIs(resultMO.Expression, binder.ReturnType);
                    }
                    else
                    {
                        condition = Expression.OrElse(
                                        Expression.Equal(resultMO.Expression, Expression.Constant(null)),
                                        Expression.TypeIs(resultMO.Expression, binder.ReturnType));
                    }

                    var checkedConvert = Expression.Condition(
                        condition,
                        convert,
                        Expression.Throw(
                            Expression.New(typeof(InvalidCastException).GetConstructor(new Type[] { typeof(string) }),
                                Expression.Call(
                                    typeof(string).GetMethod("Format", new Type[] { typeof(string), typeof(object[]) }),
                                    Expression.Constant(convertFailed),
                                    Expression.NewArrayInit(typeof(object),
                                        Expression.Condition(
                                            Expression.Equal(resultMO.Expression, Expression.Constant(null)),
                                            Expression.Constant("null"),
                                            Expression.Call(
                                                resultMO.Expression,
                                                typeof(object).GetMethod("GetType")
                                            ),
                                            typeof(object)
                                        )
                                    )
                                )
                            ),
                            binder.ReturnType
                        ),
                        binder.ReturnType
                    );
#else
                    var checkedConvert = convert;
#endif

                    resultMO = new DynamicMetaObject(checkedConvert, resultMO.Restrictions);
                }

                if(fallbackInvoke != null)
                {
                    resultMO = fallbackInvoke(resultMO);
                }

                var callDynamic = new DynamicMetaObject(
                    Expression.Block(
                        new[] { result, callArgs },
                        methodName != "TryBinaryOperation" ? Expression.Assign(callArgs, Expression.NewArrayInit(typeof(object), callArgsValue)) : Expression.Assign(callArgs, callArgsValue[0]),
                        Expression.Condition(
                            Expression.Call(
                                GetLimitedSelf(),
                                typeof(DynamicSettings).GetMethod(methodName),
                                BuildCallArgs(
                                    binder,
                                    args,
                                    callArgs,
                                    result
                                )
                            ),
                            Expression.Block(
                                methodName != "TryBinaryOperation" ? ReferenceArgAssign(callArgs, args) : Expression.Empty(),
                                resultMO.Expression
                            ),
                            fallbackResult.Expression,
                            binder.ReturnType
                        )
                    ),
                    GetRestrictions().Merge(resultMO.Restrictions).Merge(fallbackResult.Restrictions)
                );
                return callDynamic;
            }


            /// <summary> 
            /// Helper method for generating a MetaObject which calls a 
            /// specific method on Dynamic, but uses one of the arguments for
            /// the result. 
            ///
            /// args is either an array of arguments to be passed
            /// to the method as an object[] or NoArgs to signify that
            /// the target method takes no parameters. 
            /// </summary>
            private DynamicMetaObject CallMethodReturnLast(string methodName, DynamicMetaObjectBinder binder, Expression[] args, Expression value, Fallback fallback)
            {
                // 
                // First, call fallback to do default binding
                // This produces either an error or a call to a .NET member 
                //
                DynamicMetaObject fallbackResult = fallback(null);

                // 
                // Build a new expression like:
                // { 
                //   object result; 
                //   TrySetMember(payload, result = value) ? result : fallbackResult
                // } 
                //

                var result = Expression.Parameter(typeof(object), null);
                var callArgs = Expression.Parameter(typeof(object[]), null);
                var callArgsValue = GetConvertedArgs(args);

                var callDynamic = new DynamicMetaObject(
                    Expression.Block(
                        new[] { result, callArgs },
                        Expression.Assign(callArgs, Expression.NewArrayInit(typeof(object), callArgsValue)),
                        Expression.Condition(
                            Expression.Call(
                                GetLimitedSelf(),
                                typeof(DynamicSettings).GetMethod(methodName),
                                BuildCallArgs(
                                    binder,
                                    args,
                                    callArgs,
                                    Expression.Assign(result, Expression.Convert(value, typeof(object)))
                                )
                            ),
                            Expression.Block(
                                ReferenceArgAssign(callArgs, args),
                                result
                            ),
                            fallbackResult.Expression,
                            typeof(object)
                        )
                    ),
                    GetRestrictions().Merge(fallbackResult.Restrictions)
                );

                // 
                // Now, call fallback again using our new MO as the error 
                // When we do this, one of two things can happen:
                //   1. Binding will succeed, and it will ignore our call to 
                //      the dynamic method, OR
                //   2. Binding will fail, and it will use the MO we created
                //      above.
                // 
                return fallback(callDynamic);
            }


            /// <summary> 
            /// Helper method for generating a MetaObject which calls a
            /// specific method on Dynamic, but uses one of the arguments for
            /// the result.
            /// 
            /// args is either an array of arguments to be passed
            /// to the method as an object[] or NoArgs to signify that 
            /// the target method takes no parameters. 
            /// </summary>
            private DynamicMetaObject CallMethodNoResult(string methodName, DynamicMetaObjectBinder binder, Expression[] args, Fallback fallback)
            {
                //
                // First, call fallback to do default binding
                // This produces either an error or a call to a .NET member
                // 
                DynamicMetaObject fallbackResult = fallback(null);
                var callArgs = Expression.Parameter(typeof(object[]), null);
                var callArgsValue = GetConvertedArgs(args);

                // 
                // Build a new expression like:
                //   if (TryDeleteMember(payload)) { } else { fallbackResult }
                //
                var callDynamic = new DynamicMetaObject(
                    Expression.Block(
                        new[] { callArgs },
                        Expression.Assign(callArgs, Expression.NewArrayInit(typeof(object), callArgsValue)),
                        Expression.Condition(
                            Expression.Call(
                                GetLimitedSelf(),
                                typeof(DynamicSettings).GetMethod(methodName),
                                BuildCallArgs(
                                    binder,
                                    args,
                                    callArgs,
                                    null
                                )
                            ),
                            Expression.Block(
                                ReferenceArgAssign(callArgs, args),
                                Expression.Empty()
                            ),
                            fallbackResult.Expression,
                            typeof(void)
                        )
                    ),
                    GetRestrictions().Merge(fallbackResult.Restrictions)
                );

                //
                // Now, call fallback again using our new MO as the error 
                // When we do this, one of two things can happen:
                //   1. Binding will succeed, and it will ignore our call to 
                //      the dynamic method, OR 
                //   2. Binding will fail, and it will use the MO we created
                //      above. 
                //
                return fallback(callDynamic);
            }

            /// <summary>
            /// Checks if the derived type has overridden the specified method.  If there is no 
            /// implementation for the method provided then Dynamic falls back to the base class 
            /// behavior which lets the call site determine how the binder is performed.
            /// </summary> 
            private bool IsOverridden(string method)
            {
                var methods = Value.GetType().GetMember(method, MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance);

                foreach(MethodInfo mi in methods)
                {
                    return true;
                    //if(mi.DeclaringType != typeof(DynamicSettings) && mi.GetBaseDefinition().DeclaringType == typeof(DynamicSettings))
                    //{
                    //    return true;
                    //}
                }

                return false;
            }

            /// <summary> 
            /// The method takes a DynamicMetaObject, and returns an instance restriction for testing null if the object
            /// holds a null value, otherwise returns a type restriction.
            /// </summary>
            internal static BindingRestrictions GetTypeRestriction(DynamicMetaObject obj)
            {
                if(obj.Value == null && obj.HasValue)
                {
                    return BindingRestrictions.GetInstanceRestriction(obj.Expression, null);
                }
                else
                {
                    return BindingRestrictions.GetTypeRestriction(obj.Expression, obj.LimitType);
                }
            }

            /// <summary> 
            /// Returns a Restrictions object which includes our current restrictions merged
            /// with a restriction limiting our type 
            /// </summary> 
            private BindingRestrictions GetRestrictions()
            {
                Debug.Assert(Restrictions == BindingRestrictions.Empty, "We don't merge, restrictions are always empty");

                return GetTypeRestriction(this);
            }

            internal static bool AreEquivalent(Type t1, Type t2)
            {
                if(!(t1 == t2))
                    return t1.IsEquivalentTo(t2);
                else
                    return true;
            }

            /// <summary>
            /// Returns our Expression converted to DynamicSettings 
            /// </summary> 
            private Expression GetLimitedSelf()
            {
                // Convert to DynamicSettings rather than LimitType, because 
                // the limit type might be non-public.
                if(AreEquivalent(Expression.Type, typeof(DynamicSettings)))
                {
                    return Expression;
                }
                return Expression.Convert(Expression, typeof(DynamicSettings));
            }

            private new DynamicSettings Value
            {
                get
                {
                    return (DynamicSettings)base.Value;
                }
            }

            // It is okay to throw NotSupported from this binder. This object
            // is only used by DynamicSettings.GetMember--it is not expected to 
            // (and cannot) implement binding semantics. It is just so the DO 
            // can use the Name and IgnoreCase properties.
            private sealed class GetBinderAdapter : GetMemberBinder
            {
                internal GetBinderAdapter(InvokeMemberBinder binder)
                    : base(binder.Name, binder.IgnoreCase)
                {
                }

                public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
                {
                    throw new NotSupportedException();
                }
            }
        }
    }

}
