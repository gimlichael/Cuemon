using System;

namespace Cuemon.Diagnostics.Assets
{
    public sealed class ArgumentNullExceptionDescriptorAttribute : ExceptionDescriptorAttribute
    {
        public ArgumentNullExceptionDescriptorAttribute() : base(typeof(ArgumentNullException))
        {
            Code = "ArgumentNullException";
            Message = TestContext.FaultDescriptor_ArgumentNullException;
        }
    }
}