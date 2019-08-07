using System.IO;
using System.Threading.Tasks;

namespace LayItOut.DocGen
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Directory.CreateDirectory("man");
            Directory.CreateDirectory("man\\assets");
            new TypesPageComposer().Compose();
            await new ComponentPageComposer("Types").Compose();
        }
    }
}
