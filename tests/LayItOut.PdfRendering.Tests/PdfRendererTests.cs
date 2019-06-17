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
                Width = 100,
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

            var textBox = new TextBox();
            textBox.AddComponent(new Label { Text = "Hello!\n", FontColor = Color.Green, Font = new Font(FontFamily.GenericMonospace, 20, FontStyle.Underline, GraphicsUnit.World) });
            textBox.AddComponent(new Label { Text = "Hi Bob, nice to see you after", FontColor = Color.Black, Font = new Font(FontFamily.GenericSansSerif, 10, GraphicsUnit.World) });
            textBox.AddComponent(new Label { Text = "20", FontColor = Color.Red, Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold, GraphicsUnit.World) });
            textBox.AddComponent(new Label { Text = "years!\n", FontColor = Color.Black, Font = new Font(FontFamily.GenericSansSerif, 10, GraphicsUnit.World) });
            textBox.AddComponent(new Label { Text = "I'm sure you'd love to see my new", FontColor = Color.Black, Font = new Font(FontFamily.GenericSansSerif, 10, GraphicsUnit.World) });
            textBox.AddComponent(new Link { Text = "website", FontColor = Color.Blue, Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Italic, GraphicsUnit.World), Href = "http://google.com" });
            content.AddComponent(new Panel
            {
                Width = SizeUnit.Unlimited,
                Height = SizeUnit.Unlimited,
                BackgroundColor = Color.LightYellow,
                Margin = new Spacer(1),
                Border = new Border(new BorderLine(1, Color.Black)),
                Padding = new Spacer(2),
                Inner = textBox
            });

            var form = new Form(content);

            var doc = new PdfDocument();
            var page = doc.AddPage();
            page.Width = 320;
            page.Height = 400;
            renderer.Render(form, page);
            PdfImageComparer.ComparePdfs("text_box", doc);
        }

        [Fact]
        public void It_should_render_text_with_different_line_heights()
        {
            var renderer = new PdfRenderer();
            var content = new HBox { Width = SizeUnit.Unlimited };
            content.AddComponent(new Panel
            {
                Width = 100,
                Height = SizeUnit.Unlimited,
                BackgroundColor = Color.Yellow,
                Margin = new Spacer(1),
                Inner = new Label
                {
                    FontColor = Color.Red,
                    Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Underline | FontStyle.Italic, GraphicsUnit.World),
                    Text = "Hello my friend!\nIt's nice to see you!\n\nWhat is a nice and sunny day, is not it?",
                    LineHeight = 1.2f
                }
            });

            content.AddComponent(new Panel
            {
                Width = 100,
                Height = SizeUnit.Unlimited,
                BackgroundColor = Color.Yellow,
                Margin = new Spacer(1),
                Inner = new Label
                {
                    FontColor = Color.Red,
                    Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Underline | FontStyle.Italic, GraphicsUnit.World),
                    Text = "Hello my friend!\nIt's nice to see you!\n\nWhat is a nice and sunny day, is not it?",
                    LineHeight = 2f
                }
            });

            content.AddComponent(new Panel
            {
                Width = 100,
                Height = SizeUnit.Unlimited,
                BackgroundColor = Color.Yellow,
                Margin = new Spacer(1),
                Inner = new Label
                {
                    FontColor = Color.Red,
                    Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Underline | FontStyle.Italic, GraphicsUnit.World),
                    Text = "Hello my friend!\nIt's nice to see you!\n\nWhat is a nice and sunny day, is not it?",
                    LineHeight = 0.8f
                }
            });

            var form = new Form(content);

            var doc = new PdfDocument();
            var page = doc.AddPage();
            page.Width = 300;
            page.Height = 400;
            renderer.Render(form, page);
            PdfImageComparer.ComparePdfs("text_box_line_height", doc);
        }

        [Fact]
        public void It_should_render_text_with_alignments()
        {
            var renderer = new PdfRenderer();

            var labelBox = new HBox { Width = SizeUnit.Unlimited };
            foreach (var align in new[] { TextAlignment.Left, TextAlignment.Right, TextAlignment.Center, TextAlignment.Justify })
            {
                labelBox.AddComponent(new Panel
                {
                    Width = 100,
                    Height = SizeUnit.Unlimited,
                    BackgroundColor = Color.Yellow,
                    Margin = new Spacer(1),
                    Border = new Border(new BorderLine(1, Color.Black)),
                    Padding = new Spacer(2),
                    Inner = new Label
                    {
                        Width = SizeUnit.Unlimited,
                        FontColor = Color.Red,
                        TextAlignment = align,
                        Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Underline | FontStyle.Italic, GraphicsUnit.World),
                        Text = "Hello my friend!\nIt's nice to see you!\n\nWhat is a nice and sunny day, is not it?"
                    }
                });
            }

            var areaBox = new HBox { Width = SizeUnit.Unlimited };
            foreach (var align in new[] { TextAlignment.Left, TextAlignment.Right, TextAlignment.Center, TextAlignment.Justify })
            {
                var textBox = new TextBox
                {
                    Width = SizeUnit.Unlimited,
                    TextAlignment = align
                };
                textBox.AddComponent(new Label
                {
                    FontColor = Color.Red,
                    Text = "Hi Bob!",
                    Font = new Font(FontFamily.GenericSerif, 10, FontStyle.Underline | FontStyle.Italic, GraphicsUnit.World)
                });

                textBox.AddComponent(new Link
                {
                    FontColor = Color.Black,
                    Text = "Check out this: ",
                    Font = new Font(FontFamily.GenericSerif, 12, FontStyle.Regular, GraphicsUnit.World)
                });
                textBox.AddComponent(new Link
                {
                    FontColor = Color.Purple,
                    Text = "great link!!!",
                    Font = new Font(FontFamily.GenericSerif, 8, FontStyle.Underline, GraphicsUnit.World),
                    Href = "http://google.com"
                });
                areaBox.AddComponent(new Panel
                {
                    Width = 100,
                    Height = SizeUnit.Unlimited,
                    BackgroundColor = Color.Green,
                    Margin = new Spacer(1),
                    Border = new Border(new BorderLine(1, Color.Black)),
                    Padding = new Spacer(2),
                    Inner = textBox
                });
            }

            var content = new VBox { Width = SizeUnit.Unlimited };
            content.AddComponent(labelBox);
            content.AddComponent(areaBox);

            var form = new Form(content);

            var doc = new PdfDocument();
            var page = doc.AddPage();
            page.Width = 400;
            page.Height = 400;
            renderer.Render(form, page);
            PdfImageComparer.ComparePdfs("text_box_align", doc);
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
