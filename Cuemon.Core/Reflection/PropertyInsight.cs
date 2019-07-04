using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cuemon.Reflection
{
    public sealed class PropertyInsight : MemberInsight<PropertyInfo>
    {
        public static PropertyInsight FromProperty(PropertyInfo property)
        {
            return new PropertyInsight(property);
        }

        public static implicit operator PropertyInsight(PropertyInfo pi)
        {
            return new PropertyInsight(pi);
        }

        public static implicit operator PropertyInfo(PropertyInsight pi)
        {
            return pi.Member;
        }

        public PropertyInsight(PropertyInfo property) : base(property)
        {
        }


        /// <summary>
        /// Determines whether the underlying <see cref="PropertyInfo"/> of this instance has been overridden.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="PropertyInfo"/> of this instance has been overridden; otherwise, <c>false</c>.</returns>
        public bool IsOverridden()
        {
            return Member.GetGetMethod().GetBaseDefinition().DeclaringType != Member.DeclaringType;
        }

        /// <summary>
        /// Determines whether the underlying <see cref="PropertyInfo"/> of this instance is considered an automatic property implementation.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="PropertyInfo"/> of this instance is considered an automatic property implementation; otherwise, <c>false</c>.</returns>
        public bool IsAutoProperty()
        {
            if (Member.GetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() != null ||
                Member.SetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
            {
                return Member.DeclaringType != null && Member.DeclaringType.GetFields(new MemberReflection(excludeInheritancePath: true)).Any(f => f.Name.Contains(FormattableString.Invariant($"<{Member.Name}>")));
            }
            return false;
        }
    }
}