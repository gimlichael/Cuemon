using System;
using Cuemon.Extensions.Collections.Generic;

namespace Cuemon.Diagnostics.Assets
{
    public class SomeClass
    {
        [ArgumentNullExceptionDescriptor]
        public string[] StringToArray(string value)
        {
            Validator.ThrowIfNull(value, nameof(value), "Null is a no-go!");
            return value.Split(',');
        }

        [ExceptionDescriptor(typeof(ArgumentNullException), Code = "ArgumentNullException", Message = "The value cannot be null (none-resource).", MessageResourceName = "FaultDescriptor_ArgumentNullException", ResourceType = typeof(TestContext))]
        public string Shuffle(string value)
        {
            Validator.ThrowIfNull(value, nameof(value), "Null is a no-go!");
            return string.Concat(value.Shuffle());
        }

        [ExceptionDescriptor(typeof(ArgumentNullException), Code = "ArgumentNullException", Message = "The value cannot be null (none-resource).")]
        public string ShuffleNoLoc(string value)
        {
            Validator.ThrowIfNull(value, nameof(value), "Null is a no-go!");
            return string.Concat(value.Shuffle());
        }
    }
}