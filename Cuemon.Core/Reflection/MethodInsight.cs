using System.Reflection;

namespace Cuemon.Reflection
{
    public sealed class MethodInsight : MemberInsight<MethodInfo>
    {
        public static MethodInsight FromMethod(MethodInfo method)
        {
            return new MethodInsight(method);
        }

        public static implicit operator MethodInsight(MethodInfo mi)
        {
            return new MethodInsight(mi);
        }

        public static implicit operator MethodInfo(MethodInsight mi)
        {
            return mi.Member;
        }

        public MethodInsight(MethodInfo method) : base(method)
        {
        }

        /// <summary>
        /// Determines whether the underlying <see cref="MethodInfo"/> of this instance has been overridden.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="MethodInfo"/> of this instance has been overridden; otherwise, <c>false</c>.</returns>
        public bool IsOverridden()
        {
            return Member.GetBaseDefinition().DeclaringType != Member.DeclaringType;
        }
    }
}