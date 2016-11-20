using System;

namespace Cuemon
{
    /// <summary>
    /// Supports the <see cref="Condition"/> in building custom scenarios using true/false propositions.
    /// </summary>
    /// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="Template"/>.</typeparam>
    public class ConditionBuilder<TTuple> where TTuple : Template
    {
        internal ConditionBuilder(TTuple tuple)
        {
            Tuple = tuple;
        }

        private TTuple Tuple { get; set; }

        #region Conditions
        internal ConditionBuilder<TTuple> When(bool value)
        {
            ConditionResult = value;
            return this;
        }

        /// <summary>
        /// Performs a a bitwise logical conjunction (AND) operation of <see cref="ConditionResult"/> and the result of <paramref name="condition"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value for <paramref name="condition"/>.</typeparam>
        /// <param name="value">The argument for <paramref name="condition"/>.</param>
        /// <param name="condition">The delegate that will evaluate <paramref name="value"/>.</param>
        /// <returns>This instance with the result of <see cref="ConditionResult"/> AND <paramref name="condition"/>.</returns>
        public ConditionBuilder<TTuple> And<TValue>(TValue value, Func<TValue, bool> condition)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            return And(condition(value));
        }

        /// <summary>
        /// Performs a a bitwise logical conjunction (AND) operation of <see cref="ConditionResult"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value of a condition that can be either <c>true</c> or <c>false</c>.</param>
        /// <returns>This instance with the result of <see cref="ConditionResult"/> AND <paramref name="value"/>.</returns>
        public ConditionBuilder<TTuple> And(bool value)
        {
            ConditionResult &= value;
            return this;
        }

        /// <summary>
        /// Performs a bitwise logical disjunction (OR) operation of <see cref="ConditionResult"/> and <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value for <paramref name="condition"/>.</typeparam>
        /// <param name="value">The argument for <paramref name="condition"/>.</param>
        /// <param name="condition">The delegate that will evaluate <paramref name="value"/>.</param>
        /// <returns>This instance with the result of <see cref="ConditionResult"/> OR <paramref name="condition"/>.</returns>
        public ConditionBuilder<TTuple> Or<TValue>(TValue value, Func<TValue, bool> condition)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            return Or(condition(value));
        }

        /// <summary>
        /// Performs a bitwise logical disjunction (OR) operation of <see cref="ConditionResult"/> and <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value of a condition that can be either <c>true</c> or <c>false</c>.</param>
        /// <returns>This instance with the result of <see cref="ConditionResult"/> OR <paramref name="value"/>.</returns>
        public ConditionBuilder<TTuple> Or(bool value)
        {
            ConditionResult |= value;
            return this;
        }
        #endregion

        #region Invokers
        /// <summary>
        /// Invokes one of two expressions depending on the value of <see cref="ConditionResult"/>.
        /// </summary>
        /// <param name="firstExpression">The delegate that is invoked when <see cref="ConditionResult"/> is <c>true</c>.</param>
        /// <param name="secondExpression">The delegate that is invoked when <see cref="ConditionResult"/> is <c>false</c>.</param>
        /// <returns>ConditionBuilder&lt;TTuple&gt;.</returns>
        public void Invoke(Action<TTuple> firstExpression, Action<TTuple> secondExpression)
        {
            Condition.FlipFlop(ConditionResult, firstExpression, secondExpression, Tuple);
        }

        /// <summary>
        /// Invokes the expressions when <see cref="ConditionResult"/> is <c>true</c>.
        /// </summary>
        /// <param name="expression">The delegate that is invoked when <see cref="ConditionResult"/> is <c>true</c>.</param>
        public void InvokeWhenTrue(Action<TTuple> expression)
        {
            if (ConditionResult) { ActionFactory.Invoke(expression, Tuple); }
        }

        /// <summary>
        /// Invokes the expressions when <see cref="ConditionResult"/> is <c>false</c>.
        /// </summary>
        /// <param name="expression">The delegate that is invoked when <see cref="ConditionResult"/> is <c>false</c>.</param>
        public void InvokeWhenFalse(Action<TTuple> expression)
        {
            if (!ConditionResult) { ActionFactory.Invoke(expression, Tuple); }
        }
        #endregion

        /// <summary>
        /// Gets the current combined result of <see cref="Condition.Initialize{TTuple,TValue}"/>, <see cref="And"/> and <see cref="Or"/>.
        /// </summary>
        /// <value>The current combined result of <see cref="Condition.Initialize{TTuple,TValue}"/>, <see cref="And"/> and <see cref="Or"/>.</value>
        public bool ConditionResult { get; private set; }
    }
}