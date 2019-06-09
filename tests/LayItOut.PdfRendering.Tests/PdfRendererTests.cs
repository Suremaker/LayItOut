using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using LayItOut.Components;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Advanced;
using Xunit;
using Image = LayItOut.Components.Image;

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
        public void It_should_render_text_with_links()
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
                Inner = new Link
                {
                    FontColor = Color.Blue,
                    Font = new Font(FontFamily.GenericSerif, 14, FontStyle.Bold, GraphicsUnit.World),
                    Text = "How are you doing today?",
                    Href = "http://google.com"
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

        [Fact]
        public void It_should_render_images()
        {
            var redBlue = CreateBitmap(Brushes.Red, Brushes.Blue, 400, 400);
            var blueYellow = CreateBitmap(Brushes.Blue, Brushes.Yellow, 30, 30);

            var renderer = new PdfRenderer();
            var content = new HBox { Width = SizeUnit.Unlimited };
            content.AddComponent(new Image { Src = redBlue, Width = 40, Height = 40 });
            content.AddComponent(new Image { Src = redBlue, Width = 10, Height = 20 });
            content.AddComponent(new Image { Src = blueYellow });
            var form = new Form(content);

            var doc = new PdfDocument();
            var page = doc.AddPage();
            page.Width = 80;
            page.Height = 40;
            renderer.Render(form, page);
            PdfImageComparer.ComparePdfs("bitmaps", doc);
        }

        private Bitmap CreateBitmap(Brush back, Brush fore, int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.High;

                g.FillEllipse(back, 0, 0, width, height);
                g.FillEllipse(fore, 0.2f * width, 0.2f * height, 0.6f * width, 0.6f * height);
            }

            return bitmap;
        }
    }
}
