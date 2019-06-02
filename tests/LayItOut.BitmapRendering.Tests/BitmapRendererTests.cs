using System.Drawing;
using System.Drawing.Imaging;
using LayItOut.Components;
using Xunit;

namespace LayItOut.BitmapRendering.Tests
{
    public class BitmapRendererTests
    {
        [Fact]
        public void It_should_render_panels_and_hboxes()
        {
            var renderer = new BitmapRenderer();

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

            using (var bmp = new Bitmap(50, 40, PixelFormat.Format24bppRgb))
            {
                renderer.Render(form, bmp);
                BitmapComparer.CompareBitmaps("panels_hbox", bmp);
            }
        }
    }
}
