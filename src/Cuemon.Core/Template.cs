using System;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Represents a <see cref="Template"/> with an empty value.
    /// </summary>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template
    {
        /// <summary>
        /// Creates a new 0-tuple, or empty tuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <returns>A 0-tuple (empty) with no value.</returns>
        public static Template CreateZero()
        {
            return new Template();
        }

        /// <summary>
        /// Creates a new 1-tuple, or single, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T">The type of the only parameter of the tuple.</typeparam>
        /// <param name="arg">The value of the only parameter of the tuple.</param>
        /// <returns>A 1-tuple (single) whose value is (arg1).</returns>
        public static Template<T> CreateOne<T>(T arg)
        {
            return new Template<T>(arg);
        }

        /// <summary>
        /// Creates a new 2-tuple, or double, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <returns>A 2-tuple (double) whose value is (arg1, arg2).</returns>
        public static Template<T1, T2> CreateTwo<T1, T2>(T1 arg1, T2 arg2)
        {
            return new Template<T1, T2>(arg1, arg2);
        }

        /// <summary>
        /// Creates a new 3-tuple, or triple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <returns>A 3-tuple (triple) whose value is (arg1, arg2, arg3).</returns>
        public static Template<T1, T2, T3> CreateThree<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            return new Template<T1, T2, T3>(arg1, arg2, arg3);
        }

        /// <summary>
        /// Creates a new 4-tuple, or quadruple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <returns>A 4-tuple (quadruple) whose value is (arg1, arg2, arg3, arg4).</returns>
        public static Template<T1, T2, T3, T4> CreateFour<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return new Template<T1, T2, T3, T4>(arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Creates a new 5-tuple, or quintuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <returns>A 5-tuple (quintuple) whose value is (arg1, arg2, arg3, arg4, arg5).</returns>
        public static Template<T1, T2, T3, T4, T5> CreateFive<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return new Template<T1, T2, T3, T4, T5>(arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Creates a new 6-tuple, or septuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <returns>A 6-tuple (sextuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6).</returns>
        public static Template<T1, T2, T3, T4, T5, T6> CreateSix<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return new Template<T1, T2, T3, T4, T5, T6>(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Creates a new 7-tuple, or septuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <returns>An 7-tuple (septuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7> CreateSeven<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7>(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Creates a new 8-tuple, or octuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <returns>An 8-tuple (octuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8> CreateEight<T1, T2, T3, T4, T5, T6, T7, T8>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Creates a new 9-tuple, or nonuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <returns>A 9-tuple (nonuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9> CreateNine<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Creates a new 10-tuple, or decuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
        /// <returns>A 10-tuple (decuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CreateTen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Creates a new 11-tuple, or undecuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
        /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
        /// <returns>A 11-tuple (undecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CreateEleven<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        /// <summary>
        /// Creates a new 12-tuple, or duodecuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
        /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
        /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
        /// <returns>A 12-tuple (duodecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CreateTwelve<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        /// <summary>
        /// Creates a new 13-tuple, or tredecuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
        /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
        /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
        /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
        /// <returns>A 13-tuple (tredecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CreateThirteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        /// <summary>
        /// Creates a new 14-tuple, or quattuordecuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
        /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
        /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
        /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
        /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
        /// <returns>A 14-tuple (quattuordecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CreateFourteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }

        /// <summary>
        /// Creates a new 15-tuple, or quindecuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
        /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
        /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
        /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
        /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
        /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
        /// <returns>A 15-tuple (quindecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CreateFifteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }

        /// <summary>
        /// Creates a new 16-tuple, or sexdecuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
        /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
        /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
        /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
        /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
        /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
        /// <param name="arg16">The value of the sixteenth parameter of the tuple.</param>
        /// <returns>A 16-tuple (sexdecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CreateSixteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        }

        /// <summary>
        /// Creates a new 17-tuple, or septendecuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T17">The type of the seventeenth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
        /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
        /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
        /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
        /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
        /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
        /// <param name="arg16">The value of the sixteenth parameter of the tuple.</param>
        /// <param name="arg17">The value of the seventeenth parameter of the tuple.</param>
        /// <returns>A 17-tuple (septendecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> CreateSeventeen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17);
        }

        /// <summary>
        /// Creates a new 18-tuple, or octodecuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T17">The type of the seventeenth parameter of the tuple.</typeparam>
        /// <typeparam name="T18">The type of the eighteenth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
        /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
        /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
        /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
        /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
        /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
        /// <param name="arg16">The value of the sixteenth parameter of the tuple.</param>
        /// <param name="arg17">The value of the seventeenth parameter of the tuple.</param>
        /// <param name="arg18">The value of the eighteenth parameter of the tuple.</param>
        /// <returns>An 18-tuple (octodecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> CreateEighteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18);
        }

        /// <summary>
        /// Creates a new 19-tuple, or novemdecuple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T17">The type of the seventeenth parameter of the tuple.</typeparam>
        /// <typeparam name="T18">The type of the eighteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T19">The type of the nineteenth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
        /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
        /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
        /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
        /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
        /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
        /// <param name="arg16">The value of the sixteenth parameter of the tuple.</param>
        /// <param name="arg17">The value of the seventeenth parameter of the tuple.</param>
        /// <param name="arg18">The value of the eighteenth parameter of the tuple.</param>
        /// <param name="arg19">The value of the nineteenth parameter of the tuple.</param>
        /// <returns>A 19-tuple (novemdecuple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> CreateNineteen<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19);
        }

        /// <summary>
        /// Creates a new 20-tuple, or viguple, representation of a <see cref="Template"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the tuple.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the tuple.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the tuple.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the tuple.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the tuple.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the tuple.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the tuple.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the tuple.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the tuple.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the tuple.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter of the tuple.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter of the tuple.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T17">The type of the seventeenth parameter of the tuple.</typeparam>
        /// <typeparam name="T18">The type of the eighteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T19">The type of the nineteenth parameter of the tuple.</typeparam>
        /// <typeparam name="T20">The type of the twentieth parameter of the tuple.</typeparam>
        /// <param name="arg1">The value of the first parameter of the tuple.</param>
        /// <param name="arg2">The value of the second parameter of the tuple.</param>
        /// <param name="arg3">The value of the third parameter of the tuple.</param>
        /// <param name="arg4">The value of the fourth parameter of the tuple.</param>
        /// <param name="arg5">The value of the fifth parameter of the tuple.</param>
        /// <param name="arg6">The value of the sixth parameter of the tuple.</param>
        /// <param name="arg7">The value of the seventh parameter of the tuple.</param>
        /// <param name="arg8">The value of the eighth parameter of the tuple.</param>
        /// <param name="arg9">The value of the ninth parameter of the tuple.</param>
        /// <param name="arg10">The value of the tenth parameter of the tuple.</param>
        /// <param name="arg11">The value of the eleventh parameter of the tuple.</param>
        /// <param name="arg12">The value of the twelfth parameter of the tuple.</param>
        /// <param name="arg13">The value of the thirteenth parameter of the tuple.</param>
        /// <param name="arg14">The value of the fourteenth parameter of the tuple.</param>
        /// <param name="arg15">The value of the fifteenth parameter of the tuple.</param>
        /// <param name="arg16">The value of the sixteenth parameter of the tuple.</param>
        /// <param name="arg17">The value of the seventeenth parameter of the tuple.</param>
        /// <param name="arg18">The value of the eighteenth parameter of the tuple.</param>
        /// <param name="arg19">The value of the nineteenth parameter of the tuple.</param>
        /// <param name="arg20">The value of the twentieth parameter of the tuple.</param>
        /// <returns>A 20-tuple (viguple) whose value is (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20).</returns>
        public static Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> CreateTwenty<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20)
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20);
        }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public virtual object[] ToArray()
        {
            return Array.Empty<object>();
        }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance concatenated with the specified <paramref name="additionalArgs"/>.
        /// </summary>
        /// <param name="additionalArgs">The additional arguments to concatenate with the objects that represent the arguments passed to this instance.</param>
        /// <returns>An array of objects that represent the arguments passed to this instance concatenated with the specified <paramref name="additionalArgs"/>.</returns>
        public object[] ToArray(params object[] additionalArgs)
        {
            return Arguments.Concat(ToArray(), additionalArgs);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Template"/> is empty.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Template"/> is empty; otherwise, <c>false</c>.</value>
        public virtual bool IsEmpty => true;

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return DelimitedString.Create(ToArray(), o =>
            {
                o.Delimiter = ", ";
                o.StringConverter = i => Generate.ObjectPortrayal(i);
            });
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template"/> object.
        /// </summary>
        /// <returns>A new <see cref="Template"/> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public virtual Template Clone()
        {
            return new Template();
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with a single generic value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1> : Template
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1)
        {
            Arg1 = arg1;
        }

        /// <summary>
        /// Gets or sets the first parameter of this instance.
        /// </summary>
        /// <value>The first parameter of this instance.</value>
        public T1 Arg1 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1 };
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Template" /> is empty.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Template" /> is empty; otherwise, <c>false</c>.</value>
        public override bool IsEmpty => false;

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1>(Arg1);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with two generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2> : Template<T1>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2) : base(arg1)
        {
            Arg2 = arg2;
        }

        /// <summary>
        /// Gets or sets the second parameter of this instance.
        /// </summary>
        /// <value>The second parameter of this instance.</value>
        public T2 Arg2 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2>(Arg1, Arg2);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with three generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3> : Template<T1, T2>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3) : base(arg1, arg2)
        {
            Arg3 = arg3;
        }

        /// <summary>
        /// Gets or sets the third parameter of this instance.
        /// </summary>
        /// <value>The third parameter of this instance.</value>
        public T3 Arg3 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3>(Arg1, Arg2, Arg3);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with four generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4> : Template<T1, T2, T3>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4) : base(arg1, arg2, arg3)
        {
            Arg4 = arg4;
        }

        /// <summary>
        /// Gets or sets the fourth parameter of this instance.
        /// </summary>
        /// <value>The fourth parameter of this instance.</value>
        public T4 Arg4 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4>(Arg1, Arg2, Arg3, Arg4);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with five generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5> : Template<T1, T2, T3, T4>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) : base(arg1, arg2, arg3, arg4)
        {
            Arg5 = arg5;
        }

        /// <summary>
        /// Gets or sets the fifth parameter of this instance.
        /// </summary>
        /// <value>The fifth parameter of this instance.</value>
        public T5 Arg5 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5>(Arg1, Arg2, Arg3, Arg4, Arg5);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with six generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6> : Template<T1, T2, T3, T4, T5>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) : base(arg1, arg2, arg3, arg4, arg5)
        {
            Arg6 = arg6;
        }

        /// <summary>
        /// Gets or sets the sixth parameter of this instance.
        /// </summary>
        /// <value>The sixth parameter of this instance.</value>
        public T6 Arg6 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with seven generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7> : Template<T1, T2, T3, T4, T5, T6>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) : base(arg1, arg2, arg3, arg4, arg5, arg6)
        {
            Arg7 = arg7;
        }

        /// <summary>
        /// Gets or sets the seventh parameter of this instance.
        /// </summary>
        /// <value>The seventh parameter of this instance.</value>
        public T7 Arg7 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with eight generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8> : Template<T1, T2, T3, T4, T5, T6, T7>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7)
        {
            Arg8 = arg8;
        }

        /// <summary>
        /// Gets or sets the eighth parameter of this instance.
        /// </summary>
        /// <value>The eighth parameter of this instance.</value>
        public T8 Arg8 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with nine generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9> : Template<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8)
        {
            Arg9 = arg9;
        }

        /// <summary>
        /// Gets or sets the ninth parameter of this instance.
        /// </summary>
        /// <value>The ninth parameter of this instance.</value>
        public T9 Arg9 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with ten generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : Template<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg10">The value of the tenth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9)
        {
            Arg10 = arg10;
        }

        /// <summary>
        /// Gets or sets the tenth parameter of this instance.
        /// </summary>
        /// <value>The tenth parameter of this instance.</value>
        public T10 Arg10 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with eleven generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg10">The value of the tenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg11">The value of the eleventh parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10)
        {
            Arg11 = arg11;
        }

        /// <summary>
        /// Gets or sets the eleventh parameter of this instance.
        /// </summary>
        /// <value>The eleventh parameter of this instance.</value>
        public T11 Arg11 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with twelve generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg10">The value of the tenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg11">The value of the eleventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg12">The value of the twelfth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11)
        {
            Arg12 = arg12;
        }

        /// <summary>
        /// Gets or sets the twelfth parameter of this instance.
        /// </summary>
        /// <value>The twelfth parameter of this instance.</value>
        public T12 Arg12 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with thirteen generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg10">The value of the tenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg11">The value of the eleventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg12">The value of the twelfth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg13">The value of the thirteenth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12)
        {
            Arg13 = arg13;
        }

        /// <summary>
        /// Gets or sets the thirteenth parameter of this instance.
        /// </summary>
        /// <value>The thirteenth parameter of this instance.</value>
        public T13 Arg13 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with fourteen generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg10">The value of the tenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg11">The value of the eleventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg12">The value of the twelfth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg13">The value of the thirteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg14">The value of the fourteenth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13)
        {
            Arg14 = arg14;
        }

        /// <summary>
        /// Gets or sets the fourteenth parameter of this instance.
        /// </summary>
        /// <value>The fourteenth parameter of this instance.</value>
        public T14 Arg14 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with fifteen generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg10">The value of the tenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg11">The value of the eleventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg12">The value of the twelfth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg13">The value of the thirteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg14">The value of the fourteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg15">The value of the fifteenth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14)
        {
            Arg15 = arg15;
        }

        /// <summary>
        /// Gets or sets the fifteenth parameter of this instance.
        /// </summary>
        /// <value>The fifteenth parameter of this instance.</value>
        public T15 Arg15 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with sixteen generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg10">The value of the tenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg11">The value of the eleventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg12">The value of the twelfth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg13">The value of the thirteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg14">The value of the fourteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg15">The value of the fifteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg16">The value of the sixteenth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15)
        {
            Arg16 = arg16;
        }

        /// <summary>
        /// Gets or sets the sixteenth parameter of this instance.
        /// </summary>
        /// <value>The sixteenth parameter of this instance.</value>
        public T16 Arg16 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with seventeen generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17> : Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg10">The value of the tenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg11">The value of the eleventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg12">The value of the twelfth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg13">The value of the thirteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg14">The value of the fourteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg15">The value of the fifteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg16">The value of the sixteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg17">The value of the seventeenth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16)
        {
            Arg17 = arg17;
        }

        /// <summary>
        /// Gets or sets the seventeenth parameter of this instance.
        /// </summary>
        /// <value>The seventeenth parameter of this instance.</value>
        public T17 Arg17 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with eighteen generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T18">The type of the eighteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18> : Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg10">The value of the tenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg11">The value of the eleventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg12">The value of the twelfth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg13">The value of the thirteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg14">The value of the fourteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg15">The value of the fifteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg16">The value of the sixteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg17">The value of the seventeenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg18">The value of the eighteenth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17)
        {
            Arg18 = arg18;
        }

        /// <summary>
        /// Gets or sets the eighteenth parameter of this instance.
        /// </summary>
        /// <value>The eighteenth parameter of this instance.</value>
        public T18 Arg18 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with nineteen generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T18">The type of the eighteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T19">The type of the nineteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19> : Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg10">The value of the tenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg11">The value of the eleventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg12">The value of the twelfth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg13">The value of the thirteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg14">The value of the fourteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg15">The value of the fifteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg16">The value of the sixteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg17">The value of the seventeenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg18">The value of the eighteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg19">The value of the nineteenth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18)
        {
            Arg19 = arg19;
        }

        /// <summary>
        /// Gets or sets the nineteenth parameter of this instance.
        /// </summary>
        /// <value>The nineteenth parameter of this instance.</value>
        public T19 Arg19 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18, Arg19 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18, Arg19);
        }
    }

    /// <summary>
    /// Represents a <see cref="Template"/> with twenty generic values.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T18">The type of the eighteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T19">The type of the nineteenth parameter of this <see cref="Template"/>.</typeparam>
    /// <typeparam name="T20">The type of the twentieth parameter of this <see cref="Template"/>.</typeparam>
    /// <remarks>
    /// Inspired by Tuple objects, Template, was chosen because of the naming conflict in newer version of the .NET Framework. 
    /// The name, Template, was inspired by the Variadic Template in C++.
    /// </remarks>
    public class Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20> : Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20}"/> class.
        /// </summary>
        /// <param name="arg1">The value of the first parameter of this <see cref="Template"/>.</param>
        /// <param name="arg2">The value of the second parameter of this <see cref="Template"/>.</param>
        /// <param name="arg3">The value of the third parameter of this <see cref="Template"/>.</param>
        /// <param name="arg4">The value of the fourth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg5">The value of the fifth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg6">The value of the sixth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg7">The value of the seventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg8">The value of the eighth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg9">The value of the ninth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg10">The value of the tenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg11">The value of the eleventh parameter of this <see cref="Template"/>.</param>
        /// <param name="arg12">The value of the twelfth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg13">The value of the thirteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg14">The value of the fourteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg15">The value of the fifteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg16">The value of the sixteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg17">The value of the seventeenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg18">The value of the eighteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg19">The value of the nineteenth parameter of this <see cref="Template"/>.</param>
        /// <param name="arg20">The value of the twentieth parameter of this <see cref="Template"/>.</param>
        public Template(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20) : base(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19)
        {
            Arg20 = arg20;
        }

        /// <summary>
        /// Gets or sets the twentieth parameter of this instance.
        /// </summary>
        /// <value>The twentieth parameter of this instance.</value>
        public T20 Arg20 { get; set; }

        /// <summary>
        /// Returns an array of objects that represent the arguments passed to this instance.
        /// </summary>
        /// <returns>An array of objects that represent the arguments passed to this instance. Returns an empty array if the current instance was constructed with no generic arguments.</returns>
        public override object[] ToArray()
        {
            return new object[] { Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18, Arg19, Arg20 };
        }

        /// <summary>
        /// Creates a shallow copy of the current <see cref="Template" /> object.
        /// </summary>
        /// <returns>A new <see cref="Template" /> that is a copy of this instance.</returns>
        /// <remarks>When thread safety is required this is the method to invoke.</remarks>
        public override Template Clone()
        {
            return new Template<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20>(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11, Arg12, Arg13, Arg14, Arg15, Arg16, Arg17, Arg18, Arg19, Arg20);
        }
    }
}