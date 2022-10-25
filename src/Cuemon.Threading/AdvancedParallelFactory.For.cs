﻿using System;

namespace Cuemon.Threading
{
    public static partial class AdvancedParallelFactory
    {
        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
        /// <param name="rules">The rules of a for-loop control flow statement.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rules"/> cannot be null -or-
        /// <paramref name="worker"/> cannot be null.
        /// </exception>
        public static void For<TOperand>(ForLoopRuleset<TOperand> rules, Action<TOperand> worker, Action<AsyncTaskFactoryOptions> setup = null)
            where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
        {
            Validator.ThrowIfNull(rules);
            Validator.ThrowIfNull(worker);
            ForCore(rules, ActionFactory.Create(worker, default), setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="rules">The rules of a for-loop control flow statement.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rules"/> cannot be null -or-
        /// <paramref name="worker"/> cannot be null.
        /// </exception>
        public static void For<TOperand, T>(ForLoopRuleset<TOperand> rules, Action<TOperand, T> worker, T arg, Action<AsyncTaskFactoryOptions> setup = null)
            where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
        {
            Validator.ThrowIfNull(rules);
            Validator.ThrowIfNull(worker);
            ForCore(rules, ActionFactory.Create(worker, default, arg), setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="rules">The rules of a for-loop control flow statement.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rules"/> cannot be null -or-
        /// <paramref name="worker"/> cannot be null.
        /// </exception>
        public static void For<TOperand, T1, T2>(ForLoopRuleset<TOperand> rules, Action<TOperand, T1, T2> worker, T1 arg1, T2 arg2, Action<AsyncTaskFactoryOptions> setup = null)
            where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
        {
            Validator.ThrowIfNull(rules);
            Validator.ThrowIfNull(worker);
            ForCore(rules, ActionFactory.Create(worker, default, arg1, arg2), setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="rules">The rules of a for-loop control flow statement.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rules"/> cannot be null -or-
        /// <paramref name="worker"/> cannot be null.
        /// </exception>
        public static void For<TOperand, T1, T2, T3>(ForLoopRuleset<TOperand> rules, Action<TOperand, T1, T2, T3> worker, T1 arg1, T2 arg2, T3 arg3, Action<AsyncTaskFactoryOptions> setup = null)
            where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
        {
            Validator.ThrowIfNull(rules);
            Validator.ThrowIfNull(worker);
            ForCore(rules, ActionFactory.Create(worker, default, arg1, arg2, arg3), setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="rules">The rules of a for-loop control flow statement.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rules"/> cannot be null -or-
        /// <paramref name="worker"/> cannot be null.
        /// </exception>
        public static void For<TOperand, T1, T2, T3, T4>(ForLoopRuleset<TOperand> rules, Action<TOperand, T1, T2, T3, T4> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<AsyncTaskFactoryOptions> setup = null)
            where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
        {
            Validator.ThrowIfNull(rules);
            Validator.ThrowIfNull(worker);
            ForCore(rules, ActionFactory.Create(worker, default, arg1, arg2, arg3, arg4), setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TOperand">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="rules">The rules of a for-loop control flow statement.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="AsyncTaskFactoryOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rules"/> cannot be null -or-
        /// <paramref name="worker"/> cannot be null.
        /// </exception>
        public static void For<TOperand, T1, T2, T3, T4, T5>(ForLoopRuleset<TOperand> rules, Action<TOperand, T1, T2, T3, T4, T5> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<AsyncTaskFactoryOptions> setup = null)
            where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
        {
            Validator.ThrowIfNull(worker);
            Validator.ThrowIfNull(rules);
            ForCore(rules, ActionFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5), setup);
        }

        private static void ForCore<TWorker, TOperand>(ForLoopRuleset<TOperand> rules, ActionFactory<TWorker> workerFactory, Action<AsyncTaskFactoryOptions> setup)
            where TWorker : Template<TOperand>
            where TOperand : struct, IComparable<TOperand>, IEquatable<TOperand>, IConvertible
        {
            new ActionForSynchronousLoop<TOperand>(rules, setup).PrepareExecution(workerFactory);
        }
    }
}