using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    /// <summary>
    /// Provides ways to encapsulate and re-use existing code while adding support for parallel loops and regions.
    /// </summary>
    public static partial class ParallelTasks
    {
        private static readonly int DefaultNumberOfConcurrentWorkerThreads = 2 * Environment.ProcessorCount;

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <remarks>
        /// The following table shows the initial overloaded arguments for <see cref="For{TSource}(TSource,TSource,Action{TSource})"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Argument</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term>partitionSize</term>
        ///         <description><see cref="DefaultNumberOfConcurrentWorkerThreads"/></description>
        ///     </item>
        ///     <item>
        ///         <term>timeout</term>
        ///         <description><see cref="TimeSpan.FromMinutes"/> set to <c>2</c></description>
        ///     </item>
        ///     <item>
        ///         <term>relational</term>
        ///         <description><see cref="RelationalOperator.LessThan"/></description>
        ///     </item>
        ///     <item>
        ///         <term>assignment</term>
        ///         <description><see cref="AssignmentOperator.Addition"/></description>
        ///     </item>
        ///     <item>
        ///         <term>step</term>
        ///         <description><c>1</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource>(TSource initial, TSource repeats, Action<TSource> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource>(int partitionSize, TSource initial, TSource repeats, Action<TSource> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Action<TSource> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Action<TSource> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Action<TSource> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(body, timeout);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(body, default(TSource));
            ForCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T>(TSource initial, TSource repeats, Action<TSource, T> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T>(int partitionSize, TSource initial, TSource repeats, Action<TSource, T> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Action<TSource, T> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Action<TSource, T> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(body, timeout);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(body, default(TSource), arg);
            ForCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2>(TSource initial, TSource repeats, Action<TSource, T1, T2> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2>(int partitionSize, TSource initial, TSource repeats, Action<TSource, T1, T2> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Action<TSource, T1, T2> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Action<TSource, T1, T2> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(body, timeout);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(body, default(TSource), arg1, arg2);
            ForCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3>(TSource initial, TSource repeats, Action<TSource, T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3>(int partitionSize, TSource initial, TSource repeats, Action<TSource, T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Action<TSource, T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Action<TSource, T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(body, timeout);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(body, default(TSource), arg1, arg2, arg3);
            ForCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4>(TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4>(int partitionSize, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Action<TSource, T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(body, timeout);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4);
            ForCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5>(TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5>(int partitionSize, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Action<TSource, T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(body, timeout);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5);
            ForCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6>(TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6>(int partitionSize, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Action<TSource, T1, T2, T3, T4, T5, T6> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3, T4, T5, T6> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(body, timeout);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6);
            ForCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7>(TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7>(int partitionSize, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Action<TSource, T1, T2, T3, T4, T5, T6, T7> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="body" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3, T4, T5, T6, T7> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(body, timeout);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            ForCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8>(TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8>(int partitionSize, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(body, timeout);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            ForCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
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
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int partitionSize, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
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
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
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
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
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
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
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
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(body, timeout);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            ForCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
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
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
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
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int partitionSize, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
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
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
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
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
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
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
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
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
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
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static void For<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(body, timeout);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            ForCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        private static void ValidateFor(object body, TimeSpan timeout)
        {
            Validator.ThrowIfNull(body, nameof(body));
            Validator.ThrowIfNull(timeout, nameof(timeout));
            Validator.ThrowIfLowerThanOrEqual(timeout.Milliseconds, -1, nameof(timeout));
            Validator.ThrowIfGreaterThan(timeout.Milliseconds, int.MaxValue, nameof(timeout));
        }

        private static void ForCore<TTuple, TSource>(ActionFactory<TTuple> factory, Func<TSource, RelationalOperator, TSource, bool> condition, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, AssignmentOperator, TSource, TSource> iterator, AssignmentOperator assignment, TSource step, int partitionSize, TimeSpan timeout) 
            where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
            where TTuple : Template<TSource>
        {
            var breakout = false;
            var cts = new CancellationTokenSource(timeout);
            var aggregatedExceptions = new List<Exception>();
            while (true)
            {
                var partitioned = partitionSize;
                var queue = new List<Task>();
                for (var i = initial; condition(i, relational, repeats); i = iterator(i, assignment, step))
                {
                    factory.GenericArguments.Arg1 = i;
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
                    if (partitioned == 0)
                    {
                        initial = AssignmentUtility.Calculate(i, assignment, step);
                        break;
                    }
                }
                Task.WaitAll(queue.ToArray());
                if (partitioned > 0) { breakout = true; }
                if (breakout) { break; }
            }
            if (aggregatedExceptions.Count > 0) { throw new AggregateException(aggregatedExceptions); }
        }
    }
}