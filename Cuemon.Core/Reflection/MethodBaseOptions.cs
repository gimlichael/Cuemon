using System;
using System.Reflection;

namespace Cuemon.Reflection
{
    public class MethodBaseOptions
    {
        private string _memberName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodBaseOptions"/> class.
        /// </summary>
        public MethodBaseOptions()
        {
            Comparison = StringComparison.Ordinal;
            Flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
        }

        public BindingFlags Flags { get; set; }

        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        
        public string MemberName
        {
            get => _memberName;
            set 
            {
                Validator.ThrowIfNullOrWhitespace(value, nameof(value));
                _memberName = value;
            }
        }

        public Type[] Types { get; set; }

        public StringComparison Comparison { get; set; }
    }
}
