using System.Drawing;
using LayItOut.BitmapRendering.Tests.Helpers;
using LayItOut.Components;
using Xunit;

namespace LayItOut.BitmapRendering.Tests
{
    public class TextRenderingTests
    {
        [Fact]
        public void It_should_render_text()
        {
            var renderer = new BitmapRenderer();
            var content = new HBox { Width = SizeUnit.Unlimited };
            content.AddComponent(new Panel
            {
                Width = 100,
                Height = SizeUnit.Unlimited,
                BackgroundColor = Color.Yellow,
                Margin = new Spacer(1),
                Border = new Border(1, Color.White),
                Padding = new Spacer(2),
                Inner = new Label
                {
                    FontColor = Color.Red,
                    Font = new FontInfo(TestFontFamily.Serif, 10, FontInfoStyle.Underline | FontInfoStyle.Italic),
                    Text = "Hello my friend!\nIt's nice to see you!\n\nWhat is a nice and sunny day, is not it?"
                }
            });

            content.AddComponent(new Panel
            {
                Width = 100,
                Height = SizeUnit.Unlimited,
                BackgroundColor = Color.DarkSeaGreen,
                Margin = new Spacer(1),
                Border = new Border(1, Color.White),
                Padding = new Spacer(2),
                Inner = new Link
                {
                    FontColor = Color.Blue,
                    Font = new FontInfo(TestFontFamily.Serif, 14, FontInfoStyle.Bold),
                    Text = "How are you doing today?",
                    Href = "http://google.com"
                }
            });

            var textBox = new TextBox();
            textBox.AddComponent(new Label { Text = "Hello!\n", FontColor = Color.Green, Font = new FontInfo(TestFontFamily.Monospace, 20, FontInfoStyle.Underline) });
            textBox.AddComponent(new Label { Text = "Hi Bob, nice to see you after", FontColor = Color.Black, Font = new FontInfo(TestFontFamily.SansSerif, 10) });
            textBox.AddComponent(new Label { Text = "20", FontColor = Color.Red, Font = new FontInfo(TestFontFamily.SansSerif, 10, FontInfoStyle.Bold) });
            textBox.AddComponent(new Label { Text = "years!\n", FontColor = Color.Black, Font = new FontInfo(TestFontFamily.SansSerif, 10) });
            textBox.AddComponent(new Label { Text = "I'm sure you'd love to see my new", FontColor = Color.Black, Font = new FontInfo(TestFontFamily.SansSerif, 10) });
            textBox.AddComponent(new Link { Text = "website", FontColor = Color.Blue, Font = new FontInfo(TestFontFamily.SansSerif, 12, FontInfoStyle.Italic), Href = "http://google.com" });
            content.AddComponent(new Panel
            {
                Width = SizeUnit.Unlimited,
                Height = SizeUnit.Unlimited,
                BackgroundColor = Color.LightYellow,
                Margin = new Spacer(1),
                Border = new Border(1, Color.White),
                Padding = new Spacer(2),
                Inner = textBox
            });

            var form = new Form(content);

            var bmp = new Bitmap(320, 400);
            renderer.Render(form, bmp);
            BitmapComparer.CompareBitmaps("text_box", bmp);
        }

        [Fact]
        public void It_should_render_text_with_different_line_heights()
        {
            var renderer = new BitmapRenderer();
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
                    Font = new FontInfo(TestFontFamily.Serif, 10, FontInfoStyle.Underline | FontInfoStyle.Italic),
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
                    Font = new FontInfo(TestFontFamily.Serif, 10, FontInfoStyle.Underline | FontInfoStyle.Italic),
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
                    Font = new FontInfo(TestFontFamily.Serif, 10, FontInfoStyle.Underline | FontInfoStyle.Italic),
                    Text = "Hello my friend!\nIt's nice to see you!\n\nWhat is a nice and sunny day, is not it?",
                    LineHeight = 0.8f
                }
            });

            var form = new Form(content);

            var bmp = new Bitmap(300, 400);
            renderer.Render(form, bmp);
            BitmapComparer.CompareBitmaps("text_box_line_height", bmp);
        }

        [Fact]
        public void It_should_render_text_with_alignments()
        {
            var renderer = new BitmapRenderer();

            var labelBox = new HBox { Width = SizeUnit.Unlimited };
            foreach (var align in new[] { TextAlignment.Left, TextAlignment.Right, TextAlignment.Center, TextAlignment.Justify })
            {
                labelBox.AddComponent(new Panel
                {
                    Width = 100,
                    Height = SizeUnit.Unlimited,
                    BackgroundColor = Color.Yellow,
                    Margin = new Spacer(1),
                    Border = new Border(1, Color.White),
                    Padding = new Spacer(2),
                    Inner = new Label
                    {
                        Width = SizeUnit.Unlimited,
                        FontColor = Color.Red,
                        TextAlignment = align,
                        Font = new FontInfo(TestFontFamily.Serif, 10, FontInfoStyle.Underline | FontInfoStyle.Italic),
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
                    Font = new FontInfo(TestFontFamily.Serif, 10, FontInfoStyle.Underline | FontInfoStyle.Italic)
                });

                textBox.AddComponent(new Link
                {
                    FontColor = Color.Black,
                    Text = "Check out this: ",
                    Font = new FontInfo(TestFontFamily.Serif, 12, FontInfoStyle.Regular)
                });
                textBox.AddComponent(new Link
                {
                    FontColor = Color.Purple,
                    Text = "great link!!!",
                    Font = new FontInfo(TestFontFamily.Serif, 8, FontInfoStyle.Underline),
                    Href = "http://google.com"
                });
                areaBox.AddComponent(new Panel
                {
                    Width = 100,
                    Height = SizeUnit.Unlimited,
                    BackgroundColor = Color.Green,
                    Margin = new Spacer(1),
                    Border = new Border(1, Color.White),
                    Padding = new Spacer(2),
                    Inner = textBox
                });
            }

            var content = new VBox { Width = SizeUnit.Unlimited };
            content.AddComponent(labelBox);
            content.AddComponent(areaBox);

            var form = new Form(content);

            var bmp = new Bitmap(400, 400);
            renderer.Render(form, bmp);
            BitmapComparer.CompareBitmaps("text_box_align", bmp);
        }
    }
}