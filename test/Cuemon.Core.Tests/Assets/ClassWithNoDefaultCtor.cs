namespace Cuemon.Core.Assets
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