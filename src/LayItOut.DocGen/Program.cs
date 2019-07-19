using System.IO;

namespace LayItOut.DocGen
{
    class Program
    {
        static void Main(string[] args)
        {
            Directory.CreateDirectory("man");
            File.WriteAllText("man\\Types.md", new TypesPageComposer().Compose());
            File.WriteAllText("man\\Components.md", new ComponentPageComposer("Types").Compose());
        }
    }
}
