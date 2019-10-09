using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    public static partial class ParallelFactory
    {
        /// <summary>
        /// Provides a default implementation of a for-iterator callback method.
        /// </summary>
        /// <typeparam name="T">The type of the counter in a for-loop.</typeparam>
        /// <param name="current">The current value of the counter in a for-loop.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply as the assignment operator for left-hand operand <paramref name="current"/> and right-hand operand <paramref name="step"/>.</param>
        /// <param name="step">The value to assign to <paramref name="current"/> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <returns>The computed result of <paramref name="current"/> having the <paramref name="assignment"/> of <paramref name="step"/>.</returns>
        public static T Iterator<T>(T current, AssignmentOperator assignment, T step) where T : struct, IComparable<T>, IEquatable<T>, IConvertible
        {
            Calculator.ValidAsNumericOperand<T>();
            return Calculator.Calculate(current, assignment, step);
        }

        /// <summary>
        /// Provides a default implementation of a for-condition callback method.
        /// </summary>
        /// <typeparam name="T">The type of the counter in a for-loop.</typeparam>
        /// <param name="current">The current value of the counter in a for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply as the relational operator for left-hand operand <paramref name="current"/> and right-hand operand <paramref name="repeats"/>.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <returns><c>true</c> if <paramref name="current"/> does not meet the condition of <paramref name="relational"/> and <paramref name="repeats"/>; otherwise <c>false</c>.</returns>
        public static bool Condition<T>(T current, RelationalOperator relational, T repeats) where T : struct, IComparable<T>, IEquatable<T>, IConvertible
        {
            Calculator.ValidAsNumericOperand<T>();
            switch (relational)
            {
                case RelationalOperator.Equal:
                    return current.Equals(repeats);
                case RelationalOperator.GreaterThan:
                    return current.CompareTo(repeats) > 0;
                case RelationalOperator.GreaterThanOrEqual:
                    return current.CompareTo(repeats) >= 0;
                case RelationalOperator.LessThan:
                    return current.CompareTo(repeats) < 0;
                case RelationalOperator.LessThanOrEqual:
                    return current.CompareTo(repeats) <= 0;
                case RelationalOperator.NotEqual:
                    return !current.Equals(repeats);
                default:
                    throw new ArgumentOutOfRangeException(nameof(relational));
            }
        }

        /// <summary>
        /// Executes a parallel for loop.
        /// </summary>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync(int fromInclusive, int toExclusive, Action<int> worker, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync<T>(int fromInclusive, int toExclusive, Action<int, T> worker, T arg, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, arg, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync<T1, T2>(int fromInclusive, int toExclusive, Action<int, T1, T2> worker, T1 arg1, T2 arg2, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, arg1, arg2, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync<T1, T2, T3>(int fromInclusive, int toExclusive, Action<int, T1, T2, T3> worker, T1 arg1, T2 arg2, T3 arg3, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, arg1, arg2, arg3, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync<T1, T2, T3, T4>(int fromInclusive, int toExclusive, Action<int, T1, T2, T3, T4> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, arg1, arg2, arg3, arg4, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="fromInclusive">The start index, inclusive.</param>
        /// <param name="toExclusive">The end index, exclusive.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync<T1, T2, T3, T4, T5>(int fromInclusive, int toExclusive, Action<int, T1, T2, T3, T4, T5> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(worker, nameof(worker));
            return ForAsync(fromInclusive, RelationalOperator.LessThan, toExclusive, AssignmentOperator.Addition, 1, worker, arg1, arg2, arg3, arg4, arg5, setup: setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync<TNumber>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Action<TNumber> worker, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default);
            return ForCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync<TNumber, T>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Action<TNumber, T> worker, T arg, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default, arg);
            return ForCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync<TNumber, T1, T2>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Action<TNumber, T1, T2> worker, T1 arg1, T2 arg2, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default, arg1, arg2);
            return ForCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync<TNumber, T1, T2, T3>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Action<TNumber, T1, T2, T3> worker, T1 arg1, T2 arg2, T3 arg3, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default, arg1, arg2, arg3);
            return ForCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync<TNumber, T1, T2, T3, T4>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Action<TNumber, T1, T2, T3, T4> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default, arg1, arg2, arg3, arg4);
            return ForCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        /// <summary>
        /// Executes a parallel for loop that offers control of the loop control variable and loop sections.
        /// </summary>
        /// <typeparam name="TNumber">The type of the number used with the loop control variable.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="worker" />.</typeparam>
        /// <param name="from">The initial value of the loop control variable.</param>
        /// <param name="relation">The relation between the loop control variable <paramref name="from"/> and <paramref name="to"/>.</param>
        /// <param name="to">The conditional value of the loop control variable.</param>
        /// <param name="assignment">The assignment statement of the loop control variable using <paramref name="step"/>.</param>
        /// <param name="step">The value to assign the loop control variable.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="worker" />.</param>
        /// <param name="condition">The function delegate that represents the condition section of the for loop. Default value is <see cref="Condition{T}"/>.</param>
        /// <param name="iterator">The function delegate that represents the iterator section of the for loop. Default value is <see cref="Iterator{T}"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task ForAsync<TNumber, T1, T2, T3, T4, T5>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, Action<TNumber, T1, T2, T3, T4, T5> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Func<TNumber, RelationalOperator, TNumber, bool> condition = null, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator = null, Action<TaskFactoryOptions> setup = null)
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            Calculator.ValidAsNumericOperand<TNumber>();
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5);
            return ForCoreAsync(@from, relation, to, assignment, step, wf, condition, iterator, setup);
        }

        private static async Task ForCoreAsync<TWorker, TNumber>(TNumber from, RelationalOperator relation, TNumber to, AssignmentOperator assignment, TNumber step, ActionFactory<TWorker> workerFactory, Func<TNumber, RelationalOperator, TNumber, bool> condition, Func<TNumber, AssignmentOperator, TNumber, TNumber> iterator, Action<TaskFactoryOptions> setup)
            where TWorker : Template<TNumber>
            where TNumber : struct, IComparable<TNumber>, IEquatable<TNumber>, IConvertible
        {
            if (condition == null) { condition = Condition; }
            if (iterator == null) { iterator = Iterator; }

            var options = Patterns.Configure(setup);
            var exceptions = new ConcurrentBag<Exception>();

            for (; ; )
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
                            shallowWorkerFactory.GenericArguments.Arg1 = (TNumber)j;
                            shallowWorkerFactory.ExecuteMethod();
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
        }
    }
}