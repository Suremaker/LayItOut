using System.Drawing;
using LayItOut.BitmapRendering.Tests.Helpers;
using LayItOut.Components;
using Xunit;

namespace LayItOut.BitmapRendering.Tests
{
    public class ContainerRenderingTests
    {
        [Fact]
        public void It_should_render_panels_and_hboxes()
        {
            var renderer = new BitmapRenderer();

            var hbox = new HBox { Alignment = Alignment.Center };
            hbox.AddComponent(new Panel
            {
                BackgroundColor = Color.Yellow,
                Padding = new Spacer(5),
                Border = new Border(3, Color.Red),
                Alignment = new Alignment(VerticalAlignment.Bottom)
            });
            hbox.AddComponent(new Panel
            {
                BackgroundColor = Color.Green,
                Padding = new Spacer(10, 5),
                Border = new Border(2, Color.Red),
                Alignment = new Alignment(VerticalAlignment.Center)
            });
            hbox.AddComponent(new Panel
            {
                BackgroundColor = Color.Blue,
                Padding = new Spacer(15, 5),
                Border = new Border(1, Color.Red)
            });
            var panel = new Panel
            {
                Width = SizeUnit.Unlimited,
                Height = SizeUnit.Unlimited,
                BackgroundColor = Color.LightSteelBlue,
                Padding = new Spacer(2),
                Inner = hbox
            };
            var form = new Form(panel);

            var bmp = new Bitmap(50, 40);
            renderer.Render(form, bmp);
            BitmapComparer.CompareBitmaps("panels_hbox", bmp);
        }

        [Fact]
        public void It_should_render_rounded_panels()
        {
            var renderer = new BitmapRenderer();

            var vbox = new HBox();

            var borders = new[]
            {
                new []{"10 0 0 0","0 10 0 0","0 0 10 0","0 0 0 10" },
                new []{"10 10 0 0","10 0 10 0","10 0 0 10","0 0 10 10" },
                new []{"10 10 10 0","10 0 10 10","10 10 0 10","10 10 10 10" }
            };

            foreach (var boxLine in borders)
            {
                var hbox = new HBox();
                foreach (var border in boxLine)
                    hbox.AddComponent(new Panel
                    {
                        Margin = new Spacer(1),
                        BackgroundColor = Color.Blue,
                        Width = 22,
                        Height = 22,
                        BorderRadius = BorderRadius.Parse(border)
                    });
                vbox.AddComponent(hbox);
            }

            var bmp = new Bitmap(300, 40);
            renderer.Render(new Form(vbox), bmp);
            BitmapComparer.CompareBitmaps("panels_rounded", bmp);
        }

        [Fact]
        public void It_should_render_rounded_panels_with_borders()
        {
            var renderer = new BitmapRenderer();

            var vbox = new VBox();

            var borders = new[]
            {
                new []{"10 0 0 0","0 10 0 0","0 0 10 0","0 0 0 10" },
                new []{"10 10 0 0","10 0 10 0","10 0 0 10","0 0 10 10" },
                new []{"10 10 10 0","10 0 10 10","10 10 0 10","10 10 10 10" }
            };
            foreach (var boxLine in borders)
            {
                var hbox = new HBox();
                foreach (var border in boxLine)
                    hbox.AddComponent(new Panel
                    {
                        Margin = new Spacer(1),
                        BackgroundColor = Color.Orange,
                        Width = 24,
                        Height = 24,
                        Border = Border.Parse("1 white"),
                        BorderRadius = BorderRadius.Parse(border)
                    });
                vbox.AddComponent(hbox);
            }


            var bmp = new Bitmap(100, 100);
            renderer.Render(new Form(vbox), bmp);
            BitmapComparer.CompareBitmaps("panels_rounded_border", bmp);
        }
    }
}