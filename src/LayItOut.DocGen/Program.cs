using System.IO;

namespace LayItOut.DocGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("man");
            File.WriteAllText("man\\types.md", new TypesPageComposer().Compose());
            File.WriteAllText("man\\components.md", new ComponentPageComposer("types").Compose());
        }
    }
}
