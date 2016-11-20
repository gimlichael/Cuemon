using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// Provides support for a generic way of specifying loops while providing ways to encapsulate and re-use existing code.
    /// </summary>
    public static class LoopUtility
    {
        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the former is initialized to set <c>counter</c> with an initial value of 0 and applying relational rule <see cref="RelationalOperator.LessThan"/> and the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource>(TSource repeats, Action<TSource> method) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, (TSource)ObjectConverter.ChangeType(0, typeof(TSource)), RelationalOperator.LessThan, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource>(TSource initial, RelationalOperator relational, TSource repeats, Action<TSource> method) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource>(TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource> method) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, assignment, step, method);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the former is initialized to set <c>counter</c> with an initial value of 0 and applying relational rule <see cref="RelationalOperator.LessThan"/> and the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T>(TSource repeats, Action<TSource, T> method, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, (TSource)ObjectConverter.ChangeType(0, typeof(TSource)), RelationalOperator.LessThan, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T>(TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T> method, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T>(TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T> method, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, assignment, step, method, arg);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the former is initialized to set <c>counter</c> with an initial value of 0 and applying relational rule <see cref="RelationalOperator.LessThan"/> and the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2>(TSource repeats, Action<TSource, T1, T2> method, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, (TSource)ObjectConverter.ChangeType(0, typeof(TSource)), RelationalOperator.LessThan, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2>(TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2> method, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2>(TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2> method, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, assignment, step, method, arg1, arg2);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the former is initialized to set <c>counter</c> with an initial value of 0 and applying relational rule <see cref="RelationalOperator.LessThan"/> and the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3>(TSource repeats, Action<TSource, T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, (TSource)ObjectConverter.ChangeType(0, typeof(TSource)), RelationalOperator.LessThan, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3>(TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3>(TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, assignment, step, method, arg1, arg2, arg3);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the former is initialized to set <c>counter</c> with an initial value of 0 and applying relational rule <see cref="RelationalOperator.LessThan"/> and the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3, T4>(TSource repeats, Action<TSource, T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, (TSource)ObjectConverter.ChangeType(0, typeof(TSource)), RelationalOperator.LessThan, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3, T4>(TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3, T4>(TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, assignment, step, method, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the former is initialized to set <c>counter</c> with an initial value of 0 and applying relational rule <see cref="RelationalOperator.LessThan"/> and the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="repeats">The amount of repeats to do.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3, T4, T5>(TSource repeats, Action<TSource, T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, (TSource)ObjectConverter.ChangeType(0, typeof(TSource)), RelationalOperator.LessThan, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>, where the latter is initialized to increment <c>counter</c> by 1 on each iteration.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3, T4, T5>(TSource initial, RelationalOperator relational, TSource repeats, Action<TSource, T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, AssignmentOperator.Addition, (TSource)ObjectConverter.ChangeType(1, typeof(TSource)), method, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.<br/>
        /// This overload uses the default implementation of the necessary two callback methods of the for-loop; <see cref="Condition{T}"/> and <see cref="Iterator{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3, T4, T5>(TSource initial, RelationalOperator relational, TSource repeats, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            For(Condition, initial, relational, repeats, Iterator, assignment, step, method, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <param name="condition">The condition delegate of the for-loop that is invoked once per iteration and determines the outcome of the conditional ruleset.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="iterator">The iterator delegate of the for-loop that is invoked once per iteration and controls the steps of the <c>counter</c>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null - or - <paramref name="iterator"/> is null - or - <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource>(Func<TSource, RelationalOperator, TSource, bool> condition, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, AssignmentOperator, TSource, TSource> iterator, AssignmentOperator assignment, TSource step, Action<TSource> method) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(condition, iterator, method);
            var factory = ActionFactory.Create(method, default(TSource));
            ForCore(factory, condition, initial, relational, repeats, iterator, assignment, step);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="condition">The condition delegate of the for-loop that is invoked once per iteration and determines the outcome of the conditional ruleset.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="iterator">The iterator delegate of the for-loop that is invoked once per iteration and controls the steps of the <c>counter</c>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null - or - <paramref name="iterator"/> is null - or - <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T>(Func<TSource, RelationalOperator, TSource, bool> condition, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, AssignmentOperator, TSource, TSource> iterator, AssignmentOperator assignment, TSource step, Action<TSource, T> method, T arg) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(condition, iterator, method);
            var factory = ActionFactory.Create(method, default(TSource), arg);
            ForCore(factory, condition, initial, relational, repeats, iterator, assignment, step);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="condition">The condition delegate of the for-loop that is invoked once per iteration and determines the outcome of the conditional ruleset.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="iterator">The iterator delegate of the for-loop that is invoked once per iteration and controls the steps of the <c>counter</c>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null - or - <paramref name="iterator"/> is null - or - <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2>(Func<TSource, RelationalOperator, TSource, bool> condition, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, AssignmentOperator, TSource, TSource> iterator, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2> method, T1 arg1, T2 arg2) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(condition, iterator, method);
            AssignmentUtility.ValidAsNumericOperand<TSource>();
            var factory = ActionFactory.Create(method, default(TSource), arg1, arg2);
            ForCore(factory, condition, initial, relational, repeats, iterator, assignment, step);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="condition">The condition delegate of the for-loop that is invoked once per iteration and determines the outcome of the conditional ruleset.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="iterator">The iterator delegate of the for-loop that is invoked once per iteration and controls the steps of the <c>counter</c>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null - or - <paramref name="iterator"/> is null - or - <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3>(Func<TSource, RelationalOperator, TSource, bool> condition, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, AssignmentOperator, TSource, TSource> iterator, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(condition, iterator, method);
            var factory = ActionFactory.Create(method, default(TSource), arg1, arg2, arg3);
            ForCore(factory, condition, initial, relational, repeats, iterator, assignment, step);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="condition">The condition delegate of the for-loop that is invoked once per iteration and determines the outcome of the conditional ruleset.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="iterator">The iterator delegate of the for-loop that is invoked once per iteration and controls the steps of the <c>counter</c>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null - or - <paramref name="iterator"/> is null - or - <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3, T4>(Func<TSource, RelationalOperator, TSource, bool> condition, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, AssignmentOperator, TSource, TSource> iterator, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(condition, iterator, method);
            var factory = ActionFactory.Create(method, default(TSource), arg1, arg2, arg3, arg4);
            ForCore(factory, condition, initial, relational, repeats, iterator, assignment, step);
        }

        /// <summary>
        /// Provides a generic way of executing a for-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>counter</c> in the encapsulated for-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="condition">The condition delegate of the for-loop that is invoked once per iteration and determines the outcome of the conditional ruleset.</param>
        /// <param name="initial">The initial value of the <c>counter</c> in the for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply for the <c>condition</c> relational operator of the for-loop.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <param name="iterator">The iterator delegate of the for-loop that is invoked once per iteration and controls the steps of the <c>counter</c>.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply for the <c>iterator</c> assignment operator of the for-loop.</param>
        /// <param name="step">The value to assign the <c>counter</c> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null - or - <paramref name="iterator"/> is null - or - <paramref name="method"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <typeparamref name="TSource"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Byte"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="SByte"/>, <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/> or <see cref="UInt64"/>.
        /// </exception>
        /// <remarks>Do not use this method for time critical operations as there are quite some overhead do to validation of generic parameter <typeparamref name="TSource"/>.</remarks>
        public static void For<TSource, T1, T2, T3, T4, T5>(Func<TSource, RelationalOperator, TSource, bool> condition, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, AssignmentOperator, TSource, TSource> iterator, AssignmentOperator assignment, TSource step, Action<TSource, T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            ValidateFor(condition, iterator, method);
            var factory = ActionFactory.Create(method, default(TSource), arg1, arg2, arg3, arg4, arg5);
            ForCore(factory, condition, initial, relational, repeats, iterator, assignment, step);
        }

        private static void ValidateFor<TSource>(Func<TSource, RelationalOperator, TSource, bool> condition, Func<TSource, AssignmentOperator, TSource, TSource> iterator, object method) where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(iterator, nameof(iterator));
            Validator.ThrowIfNull(method, nameof(method));
            AssignmentUtility.ValidAsNumericOperand<TSource>();
        }

        private static void ForCore<TTuple, TSource>(ActionFactory<TTuple> factory, Func<TSource, RelationalOperator, TSource, bool> condition, TSource initial, RelationalOperator relational, TSource repeats, Func<TSource, AssignmentOperator, TSource, TSource> iterator, AssignmentOperator assignment, TSource step) 
            where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
            where TTuple : Template<TSource>
        {
            for (TSource i = initial; condition(i, relational, repeats); i = iterator(i, assignment, step))
            {
                factory.GenericArguments.Arg1 = i;
                factory.ExecuteMethod();
            }
        }

        /// <summary>
        /// Provides a generic way of executing a foreach-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated foreach-loop.</typeparam>
        /// <param name="source">The sequence that is iterated in the encapsulated foreach-loop.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null -or- <paramref name="source"/> is null.
        /// </exception>
        public static void ForEach<TSource>(IEnumerable<TSource> source, Action<TSource> method)
        {
            ValidateForEach(source, method);
            var factory = ActionFactory.Create(method, default(TSource));
            ForEachCore(factory, source);
        }

        /// <summary>
        /// Provides a generic way of executing a foreach-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated foreach-loop.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="source">The sequence that is iterated in the encapsulated foreach-loop.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null -or- <paramref name="source"/> is null.
        /// </exception>
        public static void ForEach<TSource, T>(IEnumerable<TSource> source, Action<TSource, T> method, T arg)
        {
            ValidateForEach(source, method);
            var factory = ActionFactory.Create(method, default(TSource), arg);
            ForEachCore(factory, source);
        }

        /// <summary>
        /// Provides a generic way of executing a foreach-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated foreach-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="source">The sequence that is iterated in the encapsulated foreach-loop.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null -or- <paramref name="source"/> is null.
        /// </exception>
        public static void ForEach<TSource, T1, T2>(IEnumerable<TSource> source, Action<TSource, T1, T2> method, T1 arg1, T2 arg2)
        {
            ValidateForEach(source, method);
            var factory = ActionFactory.Create(method, default(TSource), arg1, arg2);
            ForEachCore(factory, source);
        }

        /// <summary>
        /// Provides a generic way of executing a foreach-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated foreach-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="source">The sequence that is iterated in the encapsulated foreach-loop.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null -or- <paramref name="source"/> is null.
        /// </exception>
        public static void ForEach<TSource, T1, T2, T3>(IEnumerable<TSource> source, Action<TSource, T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
        {
            ValidateForEach(source, method);
            var factory = ActionFactory.Create(method, default(TSource), arg1, arg2, arg3);
            ForEachCore(factory, source);
        }

        /// <summary>
        /// Provides a generic way of executing a foreach-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated foreach-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="source">The sequence that is iterated in the encapsulated foreach-loop.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null -or- <paramref name="source"/> is null.
        /// </exception>
        public static void ForEach<TSource, T1, T2, T3, T4>(IEnumerable<TSource> source, Action<TSource, T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            ValidateForEach(source, method);
            var factory = ActionFactory.Create(method, default(TSource), arg1, arg2, arg3, arg4);
            ForEachCore(factory, source);
        }

        /// <summary>
        /// Provides a generic way of executing a foreach-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated foreach-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="source">The sequence that is iterated in the encapsulated foreach-loop.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null -or- <paramref name="source"/> is null.
        /// </exception>
        public static void ForEach<TSource, T1, T2, T3, T4, T5>(IEnumerable<TSource> source, Action<TSource, T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            ValidateForEach(source, method);
            var factory = ActionFactory.Create(method, default(TSource), arg1, arg2, arg3, arg4, arg5);
            ForEachCore(factory, source);
        }

        private static void ValidateForEach<TSource>(IEnumerable<TSource> source, object method)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(method, nameof(method));
        }

        private static void ForEachCore<TTuple, TSource>(ActionFactory<TTuple> factory, IEnumerable<TSource> elements) where TTuple : Template<TSource>
        {
            foreach (TSource element in elements)
            {
                factory.GenericArguments.Arg1 = element;
                factory.ExecuteMethod();
            }
        }

        /// <summary>
        /// Provides a generic way of executing a while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated while-loop.</typeparam>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="method"/> is null.
        /// <paramref name="condition"/> delegate does not target an instance method on <paramref name="source"/>
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <paramref name="source"/> does not match the source of the <paramref name="condition"/> delegate target.
        /// </exception>
        public static void While<TSource>(TSource source, Func<bool> condition, Action<TSource> method) where TSource : class 
        {
            ValidateWhile(source, condition, method);
            ValidateWhileReference(source, condition);
            var factory = ActionFactory.Create(method, source);
            WhileCore(factory, condition);
        }

        /// <summary>
        /// Provides a generic way of executing a while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated while-loop.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="method"/> is null.
        /// <paramref name="condition"/> delegate does not target an instance method on <paramref name="source"/>
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <paramref name="source"/> does not match the source of the <paramref name="condition"/> delegate target.
        /// </exception>
        public static void While<TSource, T>(TSource source, Func<bool> condition, Action<TSource, T> method, T arg) where TSource : class
        {
            ValidateWhile(source, condition, method);
            ValidateWhileReference(source, condition);
            var factory = ActionFactory.Create(method, source, arg);
            WhileCore(factory, condition);
        }

        /// <summary>
        /// Provides a generic way of executing a while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated while-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="method"/> is null.
        /// <paramref name="condition"/> delegate does not target an instance method on <paramref name="source"/>
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <paramref name="source"/> does not match the source of the <paramref name="condition"/> delegate target.
        /// </exception>
        public static void While<TSource, T1, T2>(TSource source, Func<bool> condition, Action<TSource, T1, T2> method, T1 arg1, T2 arg2) where TSource : class 
        {
            ValidateWhile(source, condition, method);
            ValidateWhileReference(source, condition);
            var factory = ActionFactory.Create(method, source, arg1, arg2);
            WhileCore(factory, condition);
        }

        /// <summary>
        /// Provides a generic way of executing a while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated while-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="method"/> is null.
        /// <paramref name="condition"/> delegate does not target an instance method on <paramref name="source"/>
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <paramref name="source"/> does not match the source of the <paramref name="condition"/> delegate target.
        /// </exception>
        public static void While<TSource, T1, T2, T3>(TSource source, Func<bool> condition, Action<TSource, T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3) where TSource : class
        {
            ValidateWhile(source, condition, method);
            ValidateWhileReference(source, condition);
            var factory = ActionFactory.Create(method, source, arg1, arg2, arg3);
            WhileCore(factory, condition);
        }

        /// <summary>
        /// Provides a generic way of executing a while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated while-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="method"/> is null.
        /// <paramref name="condition"/> delegate does not target an instance method on <paramref name="source"/>
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <paramref name="source"/> does not match the source of the <paramref name="condition"/> delegate target.
        /// </exception>
        public static void While<TSource, T1, T2, T3, T4>(TSource source, Func<bool> condition, Action<TSource, T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TSource : class 
        {
            ValidateWhile(source, condition, method);
            ValidateWhileReference(source, condition);
            var factory = ActionFactory.Create(method, source, arg1, arg2, arg3, arg4);
            WhileCore(factory, condition);
        }

        /// <summary>
        /// Provides a generic way of executing a while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TSource">The type of the <c>source</c> in the encapsulated while-loop.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method"/>.</typeparam>
        /// <param name="source">The object being iterated in the encapsulated while-loop by the <paramref name="condition"/> delegate.</param>
        /// <param name="condition">The condition delegate of the while-loop that is invoked once per iteration and is a member of <paramref name="source"/>.</param>
        /// <param name="method">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="method"/> is null.
        /// <paramref name="condition"/> delegate does not target an instance method on <paramref name="source"/>
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <paramref name="source"/> does not match the source of the <paramref name="condition"/> delegate target.
        /// </exception>
        public static void While<TSource, T1, T2, T3, T4, T5>(TSource source, Func<bool> condition, Action<TSource, T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TSource : class
        {
            ValidateWhile(source, condition, method);
            ValidateWhileReference(source, condition);
            var factory = ActionFactory.Create(method, source, arg1, arg2, arg3, arg4, arg5);
            WhileCore(factory, condition);
        }

        private static void ValidateWhile<TSource>(TSource source, Func<bool> condition, object method) where TSource : class
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(condition.Target, nameof(condition), "The specified condition delegate must target an instance method on the provided TSource.");
            Validator.ThrowIfNull(method, nameof(method));
        }

        internal static void ValidateWhileReference<TSource>(TSource source, Func<bool> condition)
        {
            Type conditionType = condition.Target.GetType();
            Type sourceType = source.GetType();
            if (!conditionType.FullName.Equals(sourceType.FullName, StringComparison.OrdinalIgnoreCase)) { throw new TypeArgumentException("source", string.Format(CultureInfo.InvariantCulture, "The specified TSource, {0}, does not match the source of the condition delegate target, {1}.", sourceType.FullName, conditionType.FullName)); }            
        }

        private static void WhileCore<TTuple>(ActionFactory<TTuple> factory, Func<bool> condition) where TTuple : Template
        {
            while (condition())
            {
                factory.ExecuteMethod();
            }
        }

        /// <summary>
        /// Provides a default implementation of a for-iterator callback method.
        /// </summary>
        /// <typeparam name="T">The type of the counter in a for-loop.</typeparam>
        /// <param name="current">The current value of the counter in a for-loop.</param>
        /// <param name="assignment">One of the enumeration values that specifies the rules to apply as the assignment operator for left-hand operand <paramref name="current"/> and right-hand operand <paramref name="step"/>.</param>
        /// <param name="step">The value to assign to <paramref name="current"/> according to the rule specified by <paramref name="assignment"/>.</param>
        /// <returns>The computed result of <paramref name="current"/> having the <paramref name="assignment"/> of <paramref name="step"/>.</returns>
        public static T Iterator<T>(T current, AssignmentOperator assignment, T step) where T : struct , IComparable<T>, IEquatable<T>, IConvertible
        {
            AssignmentUtility.ValidAsNumericOperand<T>();
            return AssignmentUtility.Calculate(current, assignment, step);
        }

        /// <summary>
        /// Provides a default implementation of a for-condition callback method.
        /// </summary>
        /// <typeparam name="T">The type of the counter in a for-loop.</typeparam>
        /// <param name="current">The current value of the counter in a for-loop.</param>
        /// <param name="relational">One of the enumeration values that specifies the rules to apply as the relational operator for left-hand operand <paramref name="current"/> and right-hand operand <paramref name="repeats"/>.</param>
        /// <param name="repeats">The amount of repeats to do according to the rules specified by <paramref name="relational"/>.</param>
        /// <returns><c>true</c> if <paramref name="current"/> does not meet the condition of <paramref name="relational"/> and <paramref name="repeats"/>; otherwise <c>false</c>.</returns>
        public static bool Condition<T>(T current, RelationalOperator relational, T repeats) where T : struct , IComparable<T>, IEquatable<T>, IConvertible
        {
            AssignmentUtility.ValidAsNumericOperand<T>();
            return ConditionCore(current, relational, repeats);
        }

        private static bool ConditionCore<T>(T current, RelationalOperator relational, T repeats) where T : IComparable<T>, IEquatable<T>, IConvertible
        {
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
    }
}