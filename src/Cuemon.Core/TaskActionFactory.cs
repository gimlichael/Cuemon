using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon
{
    /// <summary>
    /// Provides access to factory methods for creating <see cref="TaskActionFactory{TTuple}"/> instances that encapsulate a <see cref="Task"/> based function delegate with a variable amount of generic arguments.
    /// </summary>
    public static class TaskActionFactory
    {
        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/>.
        /// </summary>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/>.</returns>
        public static TaskActionFactory<Template> Create(Func<CancellationToken, Task> method)
        {
            return new TaskActionFactory<Template>((tuple, token) => method(token), Template.CreateZero(), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and one generic argument.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and one generic argument.</returns>
        public static TaskActionFactory<Template<T>> Create<T>(Func<T, CancellationToken, Task> method, T arg)
        {
            return new TaskActionFactory<Template<T>>((tuple, token) => method(tuple.Arg1, token), Template.CreateOne(arg), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and two generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and two generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2>> Create<T1, T2>(Func<T1, T2, CancellationToken, Task> method, T1 arg1, T2 arg2)
        {
            return new TaskActionFactory<Template<T1, T2>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, token), Template.CreateTwo(arg1, arg2), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and three generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and three generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3>> Create<T1, T2, T3>(Func<T1, T2, T3, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3)
        {
            return new TaskActionFactory<Template<T1, T2, T3>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, token), Template.CreateThree(arg1, arg2, arg3), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and four generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and four generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4>> Create<T1, T2, T3, T4>(Func<T1, T2, T3, T4, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, token), Template.CreateFour(arg1, arg2, arg3, arg4), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and five generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and five generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4, T5>> Create<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4, T5>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, token), Template.CreateFive(arg1, arg2, arg3, arg4, arg5), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and six generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and six generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4, T5, T6>> Create<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4, T5, T6>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, token), Template.CreateSix(arg1, arg2, arg3, arg4, arg5, arg6), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and seven generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and seven generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7>> Create<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, token), Template.CreateSeven(arg1, arg2, arg3, arg4, arg5, arg6, arg7), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and eight generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and eight generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, token), Template.CreateEight(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and nine generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and nine generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, token), Template.CreateNine(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and ten generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and ten generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, token), Template.CreateTen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and eleven generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and eleven generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, token), Template.CreateEleven(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and twelfth generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and twelfth generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, token), Template.CreateTwelve(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and thirteen generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg13">The thirteenth parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and thirteen generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, token), Template.CreateThirteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and fourteen generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg13">The thirteenth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg14">The fourteenth parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and fourteen generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, token), Template.CreateFourteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), method);
        }

        /// <summary>
        /// Creates a new <see cref="TaskActionFactory{TTuple}"/> instance encapsulating the specified <paramref name="method"/> and fifteen generic arguments.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg11">The eleventh parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg12">The twelfth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg13">The thirteenth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg14">The fourteenth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg15">The fifteenth parameter of the delegate <paramref name="method"/>.</param>
        /// <returns>An instance of <see cref="TaskActionFactory{TTuple}"/> object initialized with the specified <paramref name="method"/> and fifteen generic arguments.</returns>
        public static TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>> Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, CancellationToken, Task> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            return new TaskActionFactory<Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>((tuple, token) => method(tuple.Arg1, tuple.Arg2, tuple.Arg3, tuple.Arg4, tuple.Arg5, tuple.Arg6, tuple.Arg7, tuple.Arg8, tuple.Arg9, tuple.Arg10, tuple.Arg11, tuple.Arg12, tuple.Arg13, tuple.Arg14, tuple.Arg15, token), Template.CreateFifteen(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15), method);
        }
    }

    /// <summary>
    /// Provides an easy way of invoking an <see cref="Action" /> delegate regardless of the amount of parameters provided.
    /// </summary>
    /// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="Template"/>.</typeparam>
    public sealed class TaskActionFactory<TTuple> : TemplateFactory<TTuple> where TTuple : Template
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskActionFactory"/> class.
        /// </summary>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
        public TaskActionFactory(Func<TTuple, CancellationToken, Task> method, TTuple tuple) : this(method, tuple, method)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskActionFactory{TTuple}"/> class.
        /// </summary>
        /// <param name="method">The <see cref="Task"/> based function delegate to invoke.</param>
        /// <param name="tuple">The n-tuple argument of <paramref name="method"/>.</param>
        /// <param name="originalDelegate">The original delegate wrapped by <paramref name="method"/>.</param>
        internal TaskActionFactory(Func<TTuple, CancellationToken, Task> method, TTuple tuple, Delegate originalDelegate) : base(tuple, originalDelegate != null)
        {
            Method = method;
            DelegateInfo = Infrastructure.ResolveDelegateInfo(method, originalDelegate);
        }

        /// <summary>
        /// Gets the delegate to invoke.
        /// </summary>
        /// <value>The delegate to invoke.</value>
        private Func<TTuple, CancellationToken, Task> Method { get; }

        /// <summary>
        /// Executes the delegate associated with this instance.
        /// </summary>
        /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// No delegate was specified on the factory.
        /// </exception>
        /// <exception cref="OperationCanceledException">
        /// The <paramref name="ct"/> was canceled.
        /// </exception>
        public Task ExecuteMethodAsync(CancellationToken ct)
        {
            ThrowIfNoValidDelegate(Condition.IsNull(Method));
            ct.ThrowIfCancellationRequested();
            return Method.Invoke(GenericArguments, ct);
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="TaskActionFactory{TTuple}"/> object.
        /// </summary>
        /// <returns>A new <see cref="TaskActionFactory{TTuple}"/> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override TemplateFactory<TTuple> Clone()
        {
            return new TaskActionFactory<TTuple>(Method, GenericArguments.Clone() as TTuple);
        }
    }
}