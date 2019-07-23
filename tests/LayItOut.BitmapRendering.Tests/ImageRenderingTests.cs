using System.Drawing;
using System.Drawing.Drawing2D;
using LayItOut.Attributes;
using LayItOut.BitmapRendering.Tests.Helpers;
using LayItOut.Components;
using Xunit;
using Image = LayItOut.Components.Image;

namespace LayItOut.BitmapRendering.Tests
{
    public class ImageRenderingTests 
    {
        [Fact]
        public void It_should_render_images()
        {
            var redBlue = CreateBitmap(Brushes.Red, Brushes.Blue, 400, 400);
            var blueYellow = CreateBitmap(Brushes.Blue, Brushes.Yellow, 30, 30);

            var renderer = new BitmapRenderer();
            var content = new HBox { Width = SizeUnit.Unlimited };
            content.AddComponent(new Image { Src = redBlue, Width = 40, Height = 40 });
            content.AddComponent(new Image { Src = redBlue, Width = 10, Height = 20 });
            content.AddComponent(new Image { Src = blueYellow });
            var form = new Form(content);

            var bmp = new Bitmap(80,40);
            renderer.Render(form, bmp);
            BitmapComparer.CompareBitmaps("bitmaps", bmp);
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
