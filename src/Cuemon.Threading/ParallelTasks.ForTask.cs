using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    public static partial class ParallelTasks
    {
        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <remarks>
        /// The following table shows the initial overloaded arguments for <see cref="For{TSource}(TSource,TSource,Cuemon.Action{TSource})"/>.
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
        /// <returns>A <see cref="IReadOnlyCollection{T}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, TResult>(TSource initial, TSource repeats, Func<TSource, TResult> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, TResult>(int partitionSize, TSource initial, TSource repeats, Func<TSource, TResult> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, TResult>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Func<TSource, TResult> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, TResult> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Func<TSource, TResult> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Func<TSource, TResult> body) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateForTask<TSource>(body, timeout);
            var factory = FuncFactory.Create(body, default(TSource));
            return ForTaskCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T, TResult>(TSource initial, TSource repeats, Func<TSource, T, TResult> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T, TResult>(int partitionSize, TSource initial, TSource repeats, Func<TSource, T, TResult> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T, TResult>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Func<TSource, T, TResult> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, T, TResult> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the parallel for-loop.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body" />.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Func<TSource, T, TResult> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Func<TSource, T, TResult> body, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateForTask<TSource>(body, timeout);
            var factory = FuncFactory.Create(body, default(TSource), arg);
            return ForTaskCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, TResult>(TSource initial, TSource repeats, Func<TSource, T1, T2, TResult> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, TResult>(int partitionSize, TSource initial, TSource repeats, Func<TSource, T1, T2, TResult> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="partitionSize">The maximum number of concurrent worker threads per partition.</param>
        /// <param name="timeout">A <see cref="TimeSpan" /> that represents the time to wait for the parallel for-loop operation to complete.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, TResult>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Func<TSource, T1, T2, TResult> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, T1, T2, TResult> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Func<TSource, T1, T2, TResult> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Func<TSource, T1, T2, TResult> body, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateForTask<TSource>(body, timeout);
            var factory = FuncFactory.Create(body, default(TSource), arg1, arg2);
            return ForTaskCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body" />.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the parallel for-loop.</param>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="body">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body" />.</param>
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, TResult>(TSource initial, TSource repeats, Func<TSource, T1, T2, T3, TResult> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, TResult>(int partitionSize, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, TResult> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, TResult>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, TResult> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, T1, T2, T3, TResult> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Func<TSource, T1, T2, T3, TResult> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Func<TSource, T1, T2, T3, TResult> body, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateForTask<TSource>(body, timeout);
            var factory = FuncFactory.Create(body, default(TSource), arg1, arg2, arg3);
            return ForTaskCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, TResult>(TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, TResult>(int partitionSize, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, TResult>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, T1, T2, T3, T4, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Func<TSource, T1, T2, T3, T4, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Func<TSource, T1, T2, T3, T4, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateForTask<TSource>(body, timeout);
            var factory = FuncFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4);
            return ForTaskCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, TResult>(TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, TResult>(int partitionSize, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, TResult>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Func<TSource, T1, T2, T3, T4, T5, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Func<TSource, T1, T2, T3, T4, T5, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateForTask<TSource>(body, timeout);
            var factory = FuncFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5);
            return ForTaskCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, TResult>(TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, TResult>(int partitionSize, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, TResult>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Func<TSource, T1, T2, T3, T4, T5, T6, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5, arg6);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Func<TSource, T1, T2, T3, T4, T5, T6, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateForTask<TSource>(body, timeout);
            var factory = FuncFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6);
            return ForTaskCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, TResult>(TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, TResult>(int partitionSize, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, TResult>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Func<TSource, T1, T2, T3, T4, T5, T6, T7, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Func<TSource, T1, T2, T3, T4, T5, T6, T7, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateForTask<TSource>(body, timeout);
            var factory = FuncFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            return ForTaskCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(int partitionSize, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateForTask<TSource>(body, timeout);
            var factory = FuncFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            return ForTaskCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(int partitionSize, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateForTask<TSource>(body, timeout);
            var factory = FuncFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            return ForTaskCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(DefaultNumberOfConcurrentWorkerThreads, initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(int partitionSize, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, TimeSpan.FromMinutes(2), initial, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(int partitionSize, TimeSpan timeout, TSource initial, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, RelationalOperator.LessThan, repeats, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, AssignmentOperator.Addition, body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            return ForTask(partitionSize, timeout, initial, relational, repeats, assignment, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), body, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel for-loop while providing ways to encapsulate and re-use existing code having a return value.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the <paramref name="initial"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the <paramref name="body"/> that this function delegate encapsulates.</typeparam>
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
        /// <returns>A <see cref="IReadOnlyCollection{TResult}"/> where the result of <paramref name="body"/> has been stored in the same sequential order as <paramref name="repeats"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="body"/> is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        public static IReadOnlyCollection<TResult> ForTask<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(int partitionSize, TimeSpan timeout, TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Func<TSource, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateForTask<TSource>(body, timeout);
            var factory = FuncFactory.Create(body, default(TSource), arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            return ForTaskCore(factory, LoopUtility.Condition, initial, relational, repeats, LoopUtility.Iterator, assignment, step, partitionSize, timeout);
        }

        private static void ValidateForTask<TSource>(object body, TimeSpan timeout) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            Validator.ThrowIfNull(body, nameof(body));
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            Validator.ThrowIfNull(timeout, nameof(timeout));
            Validator.ThrowIfLowerThanOrEqual(timeout.Milliseconds, -1, nameof(timeout));
            Validator.ThrowIfGreaterThan(timeout.Milliseconds, int.MaxValue, nameof(timeout));
        }

        private static IReadOnlyCollection<TResult> ForTaskCore<TTuple, TSource, TResult>(FuncFactory<TTuple, TResult> factory, Func<TSource, RelationalOperator, TSource, bool> condition, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, AssignmentOperator, TSource, TSource> iterator, AssignmentOperator assignment, TSource step, int partitionSize, TimeSpan timeout) 
            where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible 
            where TTuple : Template<TSource>
        {
            SortedDictionary<TSource, TResult> result = new SortedDictionary<TSource, TResult>();
            CancellationTokenSource cts = new CancellationTokenSource(timeout);
            List<Exception> aggregatedExceptions = new List<Exception>();
            bool breakout = false;
            while (true)
            {
                int partitioned = partitionSize;
                List<Task> queue = new List<Task>();
                for (TSource i = initial; condition(i, relational, repeats); i = iterator(i, assignment, step))
                {
                    factory.GenericArguments.Arg1 = i;
                    var shallowFactory = factory.Clone();
                    var current = i;
                    queue.Add(Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            var item = shallowFactory.ExecuteMethod();
                            lock (result)
                            {
                                result.Add(current, item);
                            }
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
            return new ReadOnlyCollection<TResult>(result.Values.ToList());
        }
    }
}