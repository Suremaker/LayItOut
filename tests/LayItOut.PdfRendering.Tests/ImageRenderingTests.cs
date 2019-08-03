using System.Drawing;
using System.Drawing.Drawing2D;
using LayItOut.Attributes;
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
            var vBox = new VBox();
            var hBox = new HBox();
            AddComponent(hBox, new Image { Src = redBlue, Width = 40, Height = 40, Scaling = ImageScaling.Uniform });
            AddComponent(hBox, new Image { Src = redBlue, Width = 40, Height = 30, Scaling = ImageScaling.Uniform, Alignment = Alignment.Center });
            AddComponent(hBox, new Image { Src = redBlue, Width = 40, Height = 30, Scaling = ImageScaling.Uniform, Alignment = Alignment.Parse("center left") });
            AddComponent(hBox, new Image { Src = redBlue, Width = 40, Height = 30, Scaling = ImageScaling.Uniform, Alignment = Alignment.Parse("center right") });
            vBox.AddComponent(hBox);
            hBox = new HBox();
            AddComponent(hBox, new Image { Src = redBlue, Width = 30, Height = 40, Scaling = ImageScaling.Uniform, Alignment = Alignment.Center });
            AddComponent(hBox, new Image { Src = redBlue, Width = 30, Height = 40, Scaling = ImageScaling.Uniform, Alignment = Alignment.Parse("top center") });
            AddComponent(hBox, new Image { Src = redBlue, Width = 30, Height = 40, Scaling = ImageScaling.Uniform, Alignment = Alignment.Parse("bottom center") });
            vBox.AddComponent(hBox);
            hBox = new HBox();
            AddComponent(hBox, new Image { Src = redBlue, Width = 10, Height = 20, Scaling = ImageScaling.Fill });
            AddComponent(hBox, new Image { Src = blueYellow });
            AddComponent(hBox, new Image { Src = blueYellow, Width = 20, Height = 20, Alignment = Alignment.Center });
            AddComponent(hBox, new Image { Src = blueYellow, Width = 20, Height = 20, Alignment = Alignment.Parse("top left") });
            AddComponent(hBox, new Image { Src = blueYellow, Width = 20, Height = 20, Alignment = Alignment.Parse("bottom right") });
            vBox.AddComponent(hBox);
            var form = new Form(vBox);

            var doc = new PdfDocument();
            var page = doc.AddPage();
            renderer.Render(form, page, new PdfRendererOptions { AdjustPageSize = true });
            PdfImageComparer.ComparePdfs("bitmaps", doc);
        }

        private void AddComponent(IContainer container, Image image)
        {
            container.AddComponent(new Panel { Margin = Spacer.Parse("1"), Border = Border.Parse("1 green"), Inner = image });
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
