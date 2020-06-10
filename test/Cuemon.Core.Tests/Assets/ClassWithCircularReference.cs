namespace Cuemon.Core.Tests.Assets
{
    public class ClassWithCircularReference
    {
        public ClassWithCircularReference()
        {
            Reference = this;
        }

        public ClassWithCircularReference Reference { get; set; } 
    }
}