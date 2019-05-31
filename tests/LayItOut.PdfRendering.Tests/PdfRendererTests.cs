using System.Drawing;
using System.Linq;
using System.Text;
using LayItOut.Components;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Xunit;

namespace LayItOut.PdfRendering.Tests
{
    public class PdfRendererTests
    {
        static PdfRendererTests()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        [Fact]
        public void It_should_render_panels_and_hboxes()
        {
            var renderer = new PdfRenderer();

            var hbox = new HBox { Alignment = Alignment.Center };
            hbox.AddComponent(new Panel { BackgroundColor = Color.Yellow, Padding = new Spacer(5), Border = new Border(new BorderLine(3, Color.Red)), Alignment = new Alignment(VerticalAlignment.Bottom) });
            hbox.AddComponent(new Panel { BackgroundColor = Color.Green, Padding = new Spacer(10, 5), Border = new Border(new BorderLine(2, Color.Red)), Alignment = new Alignment(VerticalAlignment.Center) });
            hbox.AddComponent(new Panel { BackgroundColor = Color.Blue, Padding = new Spacer(15, 5), Border = new Border(new BorderLine(1, Color.Red)) });
            var panel = new Panel
            {
                Width = SizeUnit.Unlimited,
                Height = SizeUnit.Unlimited,
                BackgroundColor = Color.LightSteelBlue,
                Padding = new Spacer(2),
                Inner = hbox
            };
            var form = new Form(panel);

            var doc = new PdfDocument();
            var page = doc.AddPage();
            page.Width = 50;
            page.Height = 40;
            renderer.Render(form, page);
            PdfImageComparer.ComparePdfs("panels_hbox", doc);
        }
    }
}
