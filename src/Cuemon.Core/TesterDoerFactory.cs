using System;
using System.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Provides access to factory methods for creating <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instances that encapsulate a tester function delegate with a variable amount of generic arguments.
    /// </summary>
    public static class TesterDoerFactory
    {
        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/>.</returns>
        public static TesterDoerFactory<Template, TResult, TSuccess> Create<TResult, TSuccess>(TesterDoer<TResult, TSuccess> method)
        {
            return new TesterDoerFactory<Template, TResult, TSuccess>((Template tuple, out TResult result) => method(out result), TupleUtility.CreateZero(), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and one generic argument.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg">The parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and one generic argument.</returns>
        public static TesterDoerFactory<Template<T>, TResult, TSuccess> Create<T, TResult, TSuccess>(TesterDoer<T, TResult, TSuccess> method, T arg)
        {
            return new TesterDoerFactory<Template<T>, TResult, TSuccess>((Template<T> tuple, out TResult result) => method(tuple.Arg1, out result), TupleUtility.CreateOne(arg), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and two generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and two generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2>, TResult, TSuccess> Create<T1, T2, TResult, TSuccess>(TesterDoer<T1, T2, TResult, TSuccess> method, T1 arg1, T2 arg2)
        {
            return new TesterDoerFactory<Template<T1, T2>, TResult, TSuccess>((Template<T1, T2> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, out result), TupleUtility.CreateTwo(arg1, arg2), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and three generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and three generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3>, TResult, TSuccess> Create<T1, T2, T3, TResult, TSuccess>(TesterDoer<T1, T2, T3, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3)
        {
            return new TesterDoerFactory<Template<T1, T2, T3>, TResult, TSuccess>((Template<T1, T2, T3> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, out result), TupleUtility.CreateThree(arg1, arg2, arg3), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and four generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and four generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4>, TResult, TSuccess> Create<T1, T2, T3, T4, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4>, TResult, TSuccess>((Template<T1, T2, T3, T4> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, out result), TupleUtility.CreateFour(arg1, arg2, arg3, arg4), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and five generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and five generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, out result), TupleUtility.CreateFive(arg1, arg2, arg3, arg4, arg5), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and six generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and six generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, out result), TupleUtility.CreateSix(arg1, arg2, arg3, arg4, arg5, arg6), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and seven generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and seven generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, out result), TupleUtility.CreateSeven(arg1, arg2, arg3, arg4, arg5, arg6, arg7), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and eight generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and eight generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, out result), TupleUtility.CreateEight(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and nine generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and nine generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, out result), TupleUtility.CreateNine(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and ten generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and ten generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, out result), TupleUtility.CreateTen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and eleven generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and eleven generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, out result), TupleUtility.CreateEleven(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and twelfth generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and twelfth generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, out result), TupleUtility.CreateTwelve(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and thirteen generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg13">The thirteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and thirteen generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, out result), TupleUtility.CreateThirteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and fourteen generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg13">The thirteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg14">The fourteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and fourteen generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, out result), TupleUtility.CreateFourteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and fifteen generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg13">The thirteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg14">The fourteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg15">The fifteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and fifteen generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, tuple.Arg15, out result), TupleUtility.CreateFifteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and sixteen generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg13">The thirteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg14">The fourteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg15">The fifteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg16">The sixteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and sixteen generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, tuple.Arg15, tuple.Arg16, out result), TupleUtility.CreateSixteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and seventeen generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T17">The type of the seventeenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg13">The thirteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg14">The fourteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg15">The fifteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg16">The sixteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg17">The seventeenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and seventeen generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, tuple.Arg15, tuple.Arg16, tuple.Arg17, out result), TupleUtility.CreateSeventeen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and eighteen generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T17">The type of the seventeenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T18">The type of the eighteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg13">The thirteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg14">The fourteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg15">The fifteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg16">The sixteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg17">The seventeenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg18">The eighteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and eighteen generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, tuple.Arg15, tuple.Arg16, tuple.Arg17, tuple.Arg18, out result), TupleUtility.CreateEighteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and nineteen generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T17">The type of the seventeenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T18">The type of the eighteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T19">The type of the nineteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg13">The thirteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg14">The fourteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg15">The fifteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg16">The sixteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg17">The seventeenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg18">The eighteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg19">The nineteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and nineteen generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, tuple.Arg15, tuple.Arg16, tuple.Arg17, tuple.Arg18, tuple.Arg19, out result), TupleUtility.CreateNineteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19), method);
        }

        /// <summary>
        /// Creates a new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> instance encapsulating the specified <paramref name="method"/> and twenty generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T17">The type of the seventeenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T18">The type of the eighteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T19">The type of the nineteenth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T20">The type of the twentieth parameter of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg13">The thirteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg14">The fourteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg15">The fifteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg16">The sixteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg17">The seventeenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg18">The eighteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg19">The nineteenth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <param name="arg20">The twentieth parameter of the tester function delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object initialized with the specified <paramref name="method"/> and twenty generic arguments.</returns>
        public static TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>, TResult, TSuccess> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, TResult, TSuccess>(TesterDoer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, TResult, TSuccess> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20)
        {
            return new TesterDoerFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>, TResult, TSuccess>((Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> tuple, out TResult result) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, tuple.Arg15, tuple.Arg16, tuple.Arg17, tuple.Arg18, tuple.Arg19, tuple.Arg20, out result), TupleUtility.CreateTwenty(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20), method);
        }

        /// <summary>
        /// Invokes the specified delegate <paramref name="method"/> with a n-<paramref name="tuple"/> argument.
        /// </summary>
        /// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="Template"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the tester function delegate  <paramref name="method"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
        /// <param name="result">The out result value of the tester function delegate.</param>
        /// <returns>The return value that indicates success of the tester function delegate <paramref name="method"/>.</returns>
        public static TSuccess Invoke<TTuple, TResult, TSuccess>(TesterDoer<TTuple, TResult, TSuccess> method, TTuple tuple, out TResult result) where TTuple : Template
        {
            TesterDoerFactory<TTuple, TResult, TSuccess> factory = new TesterDoerFactory<TTuple, TResult, TSuccess>(method, tuple);
            return factory.ExecuteMethod(out result);
        }
    }

    /// <summary>
    /// Provides an easy way of invoking an <see cref="TesterDoer{TResult, TSuccess}" /> function delegate regardless of the amount of parameters provided.
    /// </summary>
    /// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="Template"/>.</typeparam>
    /// <typeparam name="TResult">The type of the out result value of the tester function delegate <see cref="Method"/>.</typeparam>
    /// <typeparam name="TSuccess">The type of the return value that indicates success of the tester function delegate <see cref="Method"/>.</typeparam>
    public class TesterDoerFactory<TTuple, TResult, TSuccess> : TemplateFactory<TTuple> where TTuple : Template
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TesterDoerFactory{TTuple, TResult, TSuccess}"/> class.
        /// </summary>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
        public TesterDoerFactory(TesterDoer<TTuple, TResult, TSuccess> method, TTuple tuple) : this(method, tuple, method)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TesterDoerFactory{TTuple, TResult, TSuccess}"/> class.
        /// </summary>
        /// <param name="method">The tester function delegate to invoke.</param>
        /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
        /// <param name="originalDelegate">The original delegate wrapped by <paramref name="method"/>.</param>
        internal TesterDoerFactory(TesterDoer<TTuple, TResult, TSuccess> method, TTuple tuple, Delegate originalDelegate) : base(tuple, originalDelegate != null)
        {
            Method = method;
            DelegateInfo = Infrastructure.ResolveDelegateInfo(method == null ? null : method, originalDelegate);
        }

        /// <summary>
        /// Gets the tester function delegate to invoke.
        /// </summary>
        /// <value>The <see cref="TesterDoer{TResult, TSuccess}"/> delegate to invoke.</value>
        protected TesterDoer<TTuple, TResult, TSuccess> Method { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has an assigned tester function delegate.
        /// </summary>
        /// <value><c>true</c> if this instance an assigned tester function delegate; otherwise, <c>false</c>.</value>
        public override bool HasDelegate { get { return base.HasDelegate; } }

        /// <summary>
        /// Gets the method represented by the tester function delegate.
        /// </summary>
        /// <value>A <see cref="MethodInfo" /> describing the method represented by the tester function delegate.</value>
        public override MethodInfo DelegateInfo
        {
            get { return base.DelegateInfo; }
        }

        /// <summary>
        /// Executes the tester function delegate associated with this instance.
        /// </summary>
        /// <param name="result">The out result value of the tester function delegate.</param>
        /// <returns>The return value that indicates success of the tester function delegate associated with this instance.</returns>
        public virtual TSuccess ExecuteMethod(out TResult result)
        {
            return Method(GenericArguments, out result);
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> object.
        /// </summary>
        /// <returns>A new <see cref="TesterDoerFactory{TTuple,TResult,TSuccess}"/> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public TesterDoerFactory<TTuple, TResult, TSuccess> Clone()
        {
            return new TesterDoerFactory<TTuple, TResult, TSuccess>(Method, GenericArguments.Clone() as TTuple);
        }
    }
}