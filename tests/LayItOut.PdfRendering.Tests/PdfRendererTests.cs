using System.Drawing;
using System.Text;
using LayItOut.Components;
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

        [Fact]
        public void It_should_render_text()
        {
            var renderer = new PdfRenderer();
            var content = new HBox { Width = SizeUnit.Unlimited };
            content.AddComponent(new Panel
            {
                Width = 100,
                Height = SizeUnit.Unlimited,
                BackgroundColor = Color.Yellow,
                Margin = new Spacer(1),
                Border = new Border(new BorderLine(1, Color.Black)),
                Padding = new Spacer(2),
                Inner = new Label
                {
                    FontColor = Color.Red,
                    Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Underline | FontStyle.Italic, GraphicsUnit.World),
                    Text = "Hello my friend!\nIt's nice to see you!\n\nWhat is a nice and sunny day, is not it?"
                }
            });

            content.AddComponent(new Panel
            {
                Width = SizeUnit.Unlimited,
                Height = SizeUnit.Unlimited,
                BackgroundColor = Color.DarkSeaGreen,
                Margin = new Spacer(1),
                Border = new Border(new BorderLine(1, Color.Black)),
                Padding = new Spacer(2),
                Inner = new Label
                {
                    FontColor = Color.Blue,
                    Font = new Font(FontFamily.GenericSerif, 14, FontStyle.Bold, GraphicsUnit.World),
                    Text = "How are you doing today?"
                }
            });

            var form = new Form(content);

            var doc = new PdfDocument();
            var page = doc.AddPage();
            page.Width = 200;
            page.Height = 400;
            renderer.Render(form, page);
            PdfImageComparer.ComparePdfs("text_box", doc);
        }
    }
}
