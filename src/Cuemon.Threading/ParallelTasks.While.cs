using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    public static partial class ParallelTasks
    {
        private static readonly int DefaultNumberOfConcurrentWorkerThreads = 2 * Environment.ProcessorCount;

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <remarks>
        /// The following table shows the initial overloaded arguments for <see cref="While{TSource,TResult}(TesterDoer{TSource,TResult,bool},TSource,Act{TResult})"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Argument</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term>partitionSize</term>
        ///         <description>2 + <see cref="Environment.ProcessorCount"/></description>
        ///     </item>
        ///     <item>
        ///         <term>timeout</term>
        ///         <description><see cref="TimeSpan.FromMinutes"/> set to <c>2</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult>(TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult> body)
        {
            While(DefaultNumberOfConcurrentWorkerThreads, condition, source, body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult>(int partitionSize, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult> body)
        {
            While(partitionSize, TimeSpan.FromMinutes(2), condition, source, body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel while-loop operation to complete.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult>(int partitionSize, TimeSpan timeout, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult> body)
        {
            ValidateWhile(condition, body, timeout);
            var factory = ActFactory.Create(body, default(TResult));
            TResult result;
            WhileCore(factory, condition, source, out result, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T>(TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T> body, T arg)
        {
            While(DefaultNumberOfConcurrentWorkerThreads, condition, source, body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T>(int partitionSize, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T> body, T arg)
        {
            While(partitionSize, TimeSpan.FromMinutes(2), condition, source, body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel while-loop operation to complete.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T>(int partitionSize, TimeSpan timeout, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T> body, T arg)
        {
            ValidateWhile(condition, body, timeout);
            var factory = ActFactory.Create(body, default(TResult), arg);
            TResult result;
            WhileCore(factory, condition, source, out result, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2>(TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2> body, T1 arg1, T2 arg2)
        {
            While(DefaultNumberOfConcurrentWorkerThreads, condition, source, body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2>(int partitionSize, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2> body, T1 arg1, T2 arg2)
        {
            While(partitionSize, TimeSpan.FromMinutes(2), condition, source, body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel while-loop operation to complete.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2>(int partitionSize, TimeSpan timeout, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2> body, T1 arg1, T2 arg2)
        {
            ValidateWhile(condition, body, timeout);
            var factory = ActFactory.Create(body, default(TResult), arg1, arg2);
            TResult result;
            WhileCore(factory, condition, source, out result, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3>(TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3)
        {
            While(DefaultNumberOfConcurrentWorkerThreads, condition, source, body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3>(int partitionSize, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3)
        {
            While(partitionSize, TimeSpan.FromMinutes(2), condition, source, body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel while-loop operation to complete.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3>(int partitionSize, TimeSpan timeout, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3)
        {
            ValidateWhile(condition, body, timeout);
            var factory = ActFactory.Create(body, default(TResult), arg1, arg2, arg3);
            TResult result;
            WhileCore(factory, condition, source, out result, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4>(TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            While(DefaultNumberOfConcurrentWorkerThreads, condition, source, body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4>(int partitionSize, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            While(partitionSize, TimeSpan.FromMinutes(2), condition, source, body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel while-loop operation to complete.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4>(int partitionSize, TimeSpan timeout, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            ValidateWhile(condition, body, timeout);
            var factory = ActFactory.Create(body, default(TResult), arg1, arg2, arg3, arg4);
            TResult result;
            WhileCore(factory, condition, source, out result, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5>(TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            While(DefaultNumberOfConcurrentWorkerThreads, condition, source, body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5>(int partitionSize, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            While(partitionSize, TimeSpan.FromMinutes(2), condition, source, body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel while-loop operation to complete.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5>(int partitionSize, TimeSpan timeout, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            ValidateWhile(condition, body, timeout);
            var factory = ActFactory.Create(body, default(TResult), arg1, arg2, arg3, arg4, arg5);
            TResult result;
            WhileCore(factory, condition, source, out result, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6>(TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            While(DefaultNumberOfConcurrentWorkerThreads, condition, source, body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6>(int partitionSize, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            While(partitionSize, TimeSpan.FromMinutes(2), condition, source, body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel while-loop operation to complete.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6>(int partitionSize, TimeSpan timeout, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            ValidateWhile(condition, body, timeout);
            var factory = ActFactory.Create(body, default(TResult), arg1, arg2, arg3, arg4, arg5, arg6);
            TResult result;
            WhileCore(factory, condition, source, out result, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7>(TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            While(DefaultNumberOfConcurrentWorkerThreads, condition, source, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7>(int partitionSize, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            While(partitionSize, TimeSpan.FromMinutes(2), condition, source, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel while-loop operation to complete.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7>(int partitionSize, TimeSpan timeout, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            ValidateWhile(condition, body, timeout);
            var factory = ActFactory.Create(body, default(TResult), arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            TResult result;
            WhileCore(factory, condition, source, out result, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7, T8>(TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7, T8> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            While(DefaultNumberOfConcurrentWorkerThreads, condition, source, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7, T8>(int partitionSize, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7, T8> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            While(partitionSize, TimeSpan.FromMinutes(2), condition, source, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel while-loop operation to complete.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7, T8>(int partitionSize, TimeSpan timeout, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7, T8> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            ValidateWhile(condition, body, timeout);
            var factory = ActFactory.Create(body, default(TResult), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            TResult result;
            WhileCore(factory, condition, source, out result, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            While(DefaultNumberOfConcurrentWorkerThreads, condition, source, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int partitionSize, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            While(partitionSize, TimeSpan.FromMinutes(2), condition, source, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel while-loop operation to complete.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int partitionSize, TimeSpan timeout, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            ValidateWhile(condition, body, timeout);
            var factory = ActFactory.Create(body, default(TResult), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            TResult result;
            WhileCore(factory, condition, source, out result, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            While(DefaultNumberOfConcurrentWorkerThreads, condition, source, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int partitionSize, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            While(partitionSize, TimeSpan.FromMinutes(2), condition, source, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel while-loop operation to complete.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="body"/> is null.
        /// </exception>
        public static void While<TSource, TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int partitionSize, TimeSpan timeout, TesterDoer<TSource, TResult, bool> condition, TSource source, Act<TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            ValidateWhile(condition, body, timeout);
            var factory = ActFactory.Create(body, default(TResult), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            TResult result;
            WhileCore(factory, condition, source, out result, partitionSize, timeout);
        }

        private static void ValidateWhile<TSource, TResult>(TesterDoer<TSource, TResult, bool> condition, object body, TimeSpan timeout)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(body, nameof(body));
            Validator.ThrowIfNull(timeout, nameof(timeout));
            Validator.ThrowIfLowerThanOrEqual(timeout.Milliseconds, -1, nameof(timeout));
            Validator.ThrowIfGreaterThan(timeout.Milliseconds, int.MaxValue, nameof(timeout));
        }

        private static void WhileCore<TTuple, TSource, TResult>(ActFactory<TTuple> factory, TesterDoer<TSource, TResult, bool> condition, TSource source, out TResult result, int partitionSize, TimeSpan timeout) where TTuple : Template<TResult>
        {
            bool breakout = false;
            CancellationTokenSource cts = new CancellationTokenSource(timeout);
            List<Exception> aggregatedExceptions = new List<Exception>();
            while (true)
            {
                int partitioned = partitionSize;
                List<Task> queue = new List<Task>();
                while (condition(source, out result))
                {
                    factory.GenericArguments.Arg1 = result;
                    var shallowFactory = factory.Clone();
                    queue.Add(Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            shallowFactory.ExecuteMethod();
                        }
                        catch (Exception te)
                        {
                            lock (aggregatedExceptions)
                            {
                                aggregatedExceptions.Add(te);
                            }
                        }
                    }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current));
                    partitioned--;
                    if (partitioned == 0) { break; }
                }
                Task.WaitAll(queue.ToArray());
                if (partitioned > 0) { breakout = true; }
                if (breakout) { break; }
            }
            if (aggregatedExceptions.Count > 0) { throw new AggregateException(aggregatedExceptions); }
        }
    }
}