using System;
using System.Globalization;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Provides a base-class for delegate based factories.
    /// </summary>
    /// <typeparam name="TTuple">The type of the n-tuple representation of a <see cref="Template"/>.</typeparam>
    public abstract class TemplateFactory<TTuple> where TTuple : Template
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateFactory{T}"/> class.
        /// </summary>
        /// <param name="tuple">Then-tuple representation of a <see cref="Template"/>.</param>
        /// <param name="hasDelegate"><c>true</c> if this instance has a valid delegate; otherwise, <c>false</c>.</param>
        protected TemplateFactory(TTuple tuple, bool hasDelegate)
        {
            Validator.ThrowIfNull(tuple, nameof(tuple));
            GenericArguments = tuple;
            HasDelegate = hasDelegate;
        }

        /// <summary>
        /// Gets a n-tuple representation of a <see cref="Template"/> that represents the generic arguments passed to this instance.
        /// </summary>
        /// <value>The n-tuple representation of a <see cref="Template"/> that represents the generic arguments passed to this instance.</value>
        public TTuple GenericArguments { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has an assigned delegate.
        /// </summary>
        /// <value><c>true</c> if this instance an assigned delegate; otherwise, <c>false</c>.</value>
        public virtual bool HasDelegate { get; private set; }

        /// <summary>
        /// Gets the method represented by the delegate.
        /// </summary>
        /// <value>A <see cref="MethodInfo"/> describing the method represented by the delegate. </value>
        public virtual MethodInfo DelegateInfo { get; protected set; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            if (HasDelegate)
            {
                MethodDescriptor descriptor = new MethodDescriptor(DelegateInfo);
                return descriptor.ToString();
            }
            return base.ToString();
        }

        /// <summary>
        /// Validates and throws an <see cref="InvalidOperationException"/> if this instance has no valid delegate.
        /// </summary>
        /// <param name="delegateIsNull">The value of a condition that can be either <c>true</c> or <c>false</c>.</param>
        protected void ThrowIfNoValidDelegate(bool delegateIsNull)
        {
            if (!HasDelegate) { throw new InvalidOperationException(Condition.TernaryIf(delegateIsNull, FirstExceptionString, SecondExceptionString)); }
        }

        private string FirstExceptionString()
        {
            return "There is no delegate specified on the factory.";
        }

        private string SecondExceptionString()
        {
            return string.Format(CultureInfo.InvariantCulture, "There is a delegate specified on the factory, '{0}', but it leads to a null referenced delegate wrapper.", StringConverter.FromType(GetType(), true));
        }
    }
}