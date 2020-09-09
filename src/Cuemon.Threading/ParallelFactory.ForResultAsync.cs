using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    public static partial class ParallelFactory
    {
        /// <summary>
        /// Executes a parallel for loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TResult>(int fromInclusive, int toExclusive, Func<int, TResult> worker, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForResultAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<T, TResult>(int fromInclusive, int toExclusive, Func<int, T, TResult> worker, T arg, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForResultAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, arg, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<T1, T2, TResult>(int fromInclusive, int toExclusive, Func<int, T1, T2, TResult> worker, T1 arg1, T2 arg2, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForResultAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, arg1, arg2, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<T1, T2, T3, TResult>(int fromInclusive, int toExclusive, Func<int, T1, T2, T3, TResult> worker, T1 arg1, T2 arg2, T3 arg3, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForResultAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, arg1, arg2, arg3, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<T1, T2, T3, T4, TResult>(int fromInclusive, int toExclusive, Func<int, T1, T2, T3, T4, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForResultAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, arg1, arg2, arg3, arg4, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<T1, T2, T3, T4, T5, TResult>(int fromInclusive, int toExclusive, Func<int, T1, T2, T3, T4, T5, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForResultAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, arg1, arg2, arg3, arg4, arg5, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TNumber, TResult>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Func<TNumber, TResult> worker, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default);
            return ForResultCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TNumber, T, TResult>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Func<TNumber, T, TResult> worker, T arg, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default, arg);
            return ForResultCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TNumber, T1, T2, TResult>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Func<TNumber, T1, T2, TResult> worker, T1 arg1, T2 arg2, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default, arg1, arg2);
            return ForResultCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TNumber, T1, T2, T3, TResult>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Func<TNumber, T1, T2, T3, TResult> worker, T1 arg1, T2 arg2, T3 arg3, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default, arg1, arg2, arg3);
            return ForResultCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TNumber, T1, T2, T3, T4, TResult>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Func<TNumber, T1, T2, T3, T4, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default, arg1, arg2, arg3, arg4);
            return ForResultCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as the for loop.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForResultAsync<TNumber, T1, T2, T3, T4, T5, TResult>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Func<TNumber, T1, T2, T3, T4, T5, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5);
            return ForResultCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        private static async Task<IReadOnlyCollection<TResult>> ForResultCoreAsync<TWorker, TNumber, TResult>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, FuncFactory<TWorker, TResult> workerFactory, Func<TNumber, RelationalOperator, TNumber, bool> condition, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator, Action<TaskFactoryOptions> setup)
            where TWorker : Template<TNumber>
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            if (condition == null) { condition = Condition; }
            if (iterator == null) { iterator = Iterator; }

            var options = Patterns.Configure(setup);
            var exceptions = new ConcurrentBag<Exception>();
            var result = new ConcurrentDictionary<TNumber, TResult>();

            while (true)
            {
                var workChunks = options.PartitionSize;
                var queue = new List<Task>();
                for (var i = @from; condition(i, relation, to); i = iterator(i, assignment, step))
                {
                    var shallowWorkerFactory = workerFactory.Clone();
                    queue.Add(Task.Factory.StartNew(j =>
                    {
                        try
                        {
                            var number = (TNumber)j;
                            shallowWorkerFactory.GenericArguments.Arg1 = number;
                            var presult = shallowWorkerFactory.ExecuteMethod();
                            result.TryAdd(number, presult);
                        }
                        catch (Exception e)
                        {
                            exceptions.Add(e);
                        }
                    }, i, options.CancellationToken, options.CreationOptions, options.Scheduler));
                    
                    workChunks--;
                    
                    if (workChunks == 0)
                    {
                        @from = Calculator.Calculate(i, assignment, step);
                        break;
                    }
                }
                if (queue.Count == 0) { break; }
                await Task.WhenAll(queue).ConfigureAwait(false);
                if (workChunks > 1) { break; }
            }
            if (exceptions.Count > 0) { throw new AggregateException(exceptions); }
            return new ReadOnlyCollection<TResult>(result.Values.ToList());
        }
    }
}