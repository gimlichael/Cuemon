using System;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Provide ways to work more efficient with <see cref="Activator" /> related tasks.
    /// </summary>
    public static class ActivatorUtility
    {
        /// <summary>
        /// Creates an instance of <typeparamref name="TResult"/> using the parameterless constructor.
        /// </summary>
        /// <typeparam name="TResult">The type to create.</typeparam>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult CreateInstance<TResult>()
        {
            var factory = FuncFactory.Create<TResult>(null);
            return CreateInstanceCore(factory);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> using a constructor of one parameters.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create.</typeparam>
        /// <param name="arg">The parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult CreateInstance<T, TResult>(T arg)
        {
            var factory = FuncFactory.Create<T, TResult>(null, arg);
            return CreateInstanceCore(factory);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> using a constructor of two parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult CreateInstance<T1, T2, TResult>(T1 arg1, T2 arg2)
        {
            var factory = FuncFactory.Create<T1, T2, TResult>(null, arg1, arg2);
            return CreateInstanceCore(factory);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> using a constructor of three parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <param name="arg3">The third parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult CreateInstance<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3)
        {
            var factory = FuncFactory.Create<T1, T2, T3, TResult>(null, arg1, arg2, arg3);
            return CreateInstanceCore(factory);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> using a constructor of four parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <param name="arg3">The third parameter of the constructor.</param>
        /// <param name="arg4">The fourth parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult CreateInstance<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var factory = FuncFactory.Create<T1, T2, T3, T4, TResult>(null, arg1, arg2, arg3, arg4);
            return CreateInstanceCore(factory);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> using a constructor of five parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the constructor.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <param name="arg3">The third parameter of the constructor.</param>
        /// <param name="arg4">The fourth parameter of the constructor.</param>
        /// <param name="arg5">The fifth parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult CreateInstance<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var factory = FuncFactory.Create<T1, T2, T3, T4, T5, TResult>(null, arg1, arg2, arg3, arg4, arg5);
            return CreateInstanceCore(factory);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> using a constructor of six parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the constructor.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the constructor.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <param name="arg3">The third parameter of the constructor.</param>
        /// <param name="arg4">The fourth parameter of the constructor.</param>
        /// <param name="arg5">The fifth parameter of the constructor.</param>
        /// <param name="arg6">The sixth parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult CreateInstance<T1, T2, T3, T4, T5, T6, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            var factory = FuncFactory.Create<T1, T2, T3, T4, T5, T6, TResult>(null, arg1, arg2, arg3, arg4, arg5, arg6);
            return CreateInstanceCore(factory);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> using a constructor of seven parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the constructor.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the constructor.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the constructor.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <param name="arg3">The third parameter of the constructor.</param>
        /// <param name="arg4">The fourth parameter of the constructor.</param>
        /// <param name="arg5">The fifth parameter of the constructor.</param>
        /// <param name="arg6">The sixth parameter of the constructor.</param>
        /// <param name="arg7">The seventh parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult CreateInstance<T1, T2, T3, T4, T5, T6, T7, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            var factory = FuncFactory.Create<T1, T2, T3, T4, T5, T6, T7, TResult>(null, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return CreateInstanceCore(factory);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> using a constructor of eight parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the constructor.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the constructor.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the constructor.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the constructor.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <param name="arg3">The third parameter of the constructor.</param>
        /// <param name="arg4">The fourth parameter of the constructor.</param>
        /// <param name="arg5">The fifth parameter of the constructor.</param>
        /// <param name="arg6">The sixth parameter of the constructor.</param>
        /// <param name="arg7">The seventh parameter of the constructor.</param>
        /// <param name="arg8">The eighth parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult CreateInstance<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            var factory = FuncFactory.Create<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return CreateInstanceCore(factory);
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> using a constructor of nine parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the constructor.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the constructor.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the constructor.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the constructor.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the constructor.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <param name="arg3">The third parameter of the constructor.</param>
        /// <param name="arg4">The fourth parameter of the constructor.</param>
        /// <param name="arg5">The fifth parameter of the constructor.</param>
        /// <param name="arg6">The sixth parameter of the constructor.</param>
        /// <param name="arg7">The seventh parameter of the constructor.</param>
        /// <param name="arg8">The eighth parameter of the constructor.</param>
        /// <param name="arg9">The ninth parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult CreateInstance<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            var factory = FuncFactory.Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return CreateInstanceCore(factory);
        }


        /// <summary>
        /// Creates an instance of <typeparamref name="TResult" /> using a constructor of ten parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the constructor.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the constructor.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the constructor.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the constructor.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the constructor.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the constructor.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the constructor.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the constructor.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the constructor.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the constructor.</typeparam>
        /// <typeparam name="TResult">The type to create.</typeparam>
        /// <param name="arg1">The first parameter of the constructor.</param>
        /// <param name="arg2">The second parameter of the constructor.</param>
        /// <param name="arg3">The third parameter of the constructor.</param>
        /// <param name="arg4">The fourth parameter of the constructor.</param>
        /// <param name="arg5">The fifth parameter of the constructor.</param>
        /// <param name="arg6">The sixth parameter of the constructor.</param>
        /// <param name="arg7">The seventh parameter of the constructor.</param>
        /// <param name="arg8">The eighth parameter of the constructor.</param>
        /// <param name="arg9">The ninth parameter of the constructor.</param>
        /// <param name="arg10">The tenth parameter of the constructor.</param>
        /// <returns>A reference to the newly created object.</returns>
        public static TResult CreateInstance<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            var factory = FuncFactory.Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(null, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            return CreateInstanceCore(factory);
        }

        private static TResult CreateInstanceCore<TTuple, TResult>(FuncFactory<TTuple, TResult> factory) where TTuple : Template
        {
            return (TResult)Activator.CreateInstance(typeof(TResult), factory.GenericArguments.ToArray());
        }
    }
}