namespace Cuemon.Core.Assets
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