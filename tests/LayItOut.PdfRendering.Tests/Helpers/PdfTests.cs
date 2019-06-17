using System.Text;

namespace LayItOut.PdfRendering.Tests.Helpers
{
    public class PdfTests
    {
        static PdfTests()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
    }
}