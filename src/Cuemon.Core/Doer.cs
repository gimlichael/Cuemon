namespace Cuemon
{
	/// <summary>
	/// Encapsulates a method that has no parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter. 
	/// </summary>
	/// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
	/// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
	public delegate TResult Doer<out TResult>();

	/// <summary>
	/// Encapsulates a method that has one parameter and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
	/// </summary>
	/// <typeparam name="T">The type of the parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
	/// <param name="arg">The parameter of the method that this function delegate encapsulates.</param>
	/// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T, out TResult>(T arg);

	/// <summary>
	/// Encapsulates a method that has two parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
	/// </summary>
	/// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
	/// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
	/// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
	/// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
	public delegate TResult Doer<in T1, in T2, out TResult>(T1 arg1, T2 arg2);

	/// <summary>
	/// Encapsulates a method that has three parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
	/// </summary>
	/// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
	/// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
	/// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
	/// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
	/// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
	public delegate TResult Doer<in T1, in T2, in T3, out TResult>(T1 arg1, T2 arg2, T3 arg3);

	/// <summary>
	/// Encapsulates a method that has four parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
	/// </summary>
	/// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
	/// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
	/// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
	/// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
	/// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
	/// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
	public delegate TResult Doer<in T1, in T2, in T3, in T4, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);

	/// <summary>
	/// Encapsulates a method that has five parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
	/// </summary>
	/// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="T5">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
	/// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
	/// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
	/// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
	/// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
	/// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
	/// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
	public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    /// <summary>
    /// Encapsulates a method that has six parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

    /// <summary>
    /// Encapsulates a method that has seven parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

    /// <summary>
    /// Encapsulates a method that has eight parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

    /// <summary>
    /// Encapsulates a method that has nine parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

    /// <summary>
    /// Encapsulates a method that has ten parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

    /// <summary>
    /// Encapsulates a method that has eleven parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

    /// <summary>
    /// Encapsulates a method that has twelve parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

    /// <summary>
    /// Encapsulates a method that has thirteen parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);

    /// <summary>
    /// Encapsulates a method that has fourteen parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg14">The fourteenth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

    /// <summary>
    /// Encapsulates a method that has fifteen parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg14">The fourteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg15">The fifteenth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

    /// <summary>
    /// Encapsulates a method that has sixteen parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg14">The fourteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg15">The fifteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg16">The sixteenth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);

    /// <summary>
    /// Encapsulates a method that has seventeen parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg14">The fourteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg15">The fifteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg16">The sixteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg17">The seventeenth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, in T17, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17);

    /// <summary>
    /// Encapsulates a method that has eighteen parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T18">The type of the eighteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg14">The fourteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg15">The fifteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg16">The sixteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg17">The seventeenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg18">The eighteenth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, in T17, in T18, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18);

    /// <summary>
    /// Encapsulates a method that has nineteen parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T18">The type of the eighteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T19">The type of the nineteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg14">The fourteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg15">The fifteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg16">The sixteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg17">The seventeenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg18">The eighteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg19">The nineteenth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, in T17, in T18, in T19, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19);

    /// <summary>
    /// Encapsulates a method that has twenty parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T17">The type of the seventeenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T18">The type of the eighteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T19">The type of the nineteenth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="T20">The type of the twentieth parameter of the method that this function delegate encapsulates.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the method that this function delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg3">The third parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg4">The fourth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg5">The fifth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg6">The sixth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg7">The seventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg8">The eighth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg9">The ninth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg10">The tenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg11">The eleventh parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg12">The twelfth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg13">The thirteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg14">The fourteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg15">The fifteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg16">The sixteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg17">The seventeenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg18">The eighteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg19">The nineteenth parameter of the method that this function delegate encapsulates.</param>
    /// <param name="arg20">The twentieth parameter of the method that this function delegate encapsulates.</param>
    /// <returns>The return value of the method that this function delegate encapsulates.</returns>
    /// <remarks>This function delegate is equivalent to <c>Func{TResult}</c> in .NET 3.5 (and newer) and for the same reason renamed to avoid naming conflict. The noun <c>Doer</c> was carefully selected to reflect a go-getter; a method that does something and then returns the result as part of a job. This fits perfectly for a generic function delegate.</remarks>
    public delegate TResult Doer<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, in T17, in T18, in T19, in T20, out TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, T17 arg17, T18 arg18, T19 arg19, T20 arg20);
}