using System.Drawing;
using System.Drawing.Drawing2D;
using LayItOut.Components;
using LayItOut.PdfRendering.Tests.Helpers;
using PdfSharp.Pdf;
using Xunit;
using Image = LayItOut.Components.Image;

namespace LayItOut.PdfRendering.Tests
{
    public class ImageRenderingTests : PdfTests
    {
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
