using System;
using System.Reflection;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Provides a robust way specifying binding constraints for reflection based member searching.
    /// </summary>
    public class MemberReflection
    {
        /// <summary>
        /// Defines a binding constraint that allows searching all members of a given type.
        /// </summary>
        public const BindingFlags Everything = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        /// <summary>
        /// Performs an implicit conversion from <see cref="MemberReflection"/> to <see cref="BindingFlags"/>.
        /// </summary>
        /// <param name="mr">The <see cref="MemberReflection"/> to convert.</param>
        /// <returns>A <see cref="BindingFlags"/> that is equivalent to <paramref name="mr"/>.</returns>
        public static implicit operator BindingFlags(MemberReflection mr)
        {
            return mr.Flags;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberReflection"/> class.
        /// </summary>
        public MemberReflection() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberReflection"/> class.
        /// </summary>
        /// <param name="excludePrivate">if set to <c>true</c> non-public members are excluded from the binding constraint.</param>
        /// <param name="excludeStatic">if set to <c>true</c> static members are excluded from the binding constraint.</param>
        /// <param name="excludeInheritancePath">if set to <c>true</c> derived members of a type's inheritance path are excluded from the binding constraint.</param>
        public MemberReflection(bool excludePrivate = false, bool excludeStatic = false, bool excludeInheritancePath = false) :
            this(o =>
            {
                o.ExcludeInheritancePath = excludeInheritancePath;
                o.ExcludePrivate = excludePrivate;
                o.ExcludeStatic = excludeStatic;
            })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberReflection"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="MemberReflectionOptions"/> which need to be configured.</param>
        public MemberReflection(Action<MemberReflectionOptions> setup)
        {
            var options = Patterns.Configure(setup);
            var flags = Everything;
            if (options.ExcludePrivate) { flags &= ~BindingFlags.NonPublic; }
            if (options.ExcludeStatic) { flags &= ~BindingFlags.Static; }
            if (options.ExcludeInheritancePath) { flags |= BindingFlags.DeclaredOnly; }
            Flags = flags;
        }

        /// <summary>
        /// Gets the binding constraint of this instance.
        /// </summary>
        /// <value>The binding constraint of this instance.</value>
        public BindingFlags Flags { get; }
    }
}