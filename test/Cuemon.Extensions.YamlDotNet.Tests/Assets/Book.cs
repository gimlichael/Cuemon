namespace Cuemon.Extensions.YamlDotNet.Assets
{
    public class Book
    {
        public Book()
        {
            Title = Generate.RandomString(10);
            Summary = Generate.RandomString(255);
        }

        public string Title { get; set; }

        public string Summary { get; set; }
    }
}
