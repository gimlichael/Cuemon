namespace Cuemon.Core.Tests.Assets
{
    public class ClassWithNoDefaultCtor
    {
        public ClassWithNoDefaultCtor(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}