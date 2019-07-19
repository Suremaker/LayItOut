using System.IO;

namespace LayItOut.DocGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("man");
            Directory.CreateDirectory("man\\images");
            new TypesPageComposer().Compose();
            new ComponentPageComposer("Types").Compose();
        }
    }
}
