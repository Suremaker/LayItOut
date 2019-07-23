using System;
using System.ComponentModel;
using System.Drawing;
using LayItOut.Attributes;
using LayItOut.Rendering;

namespace LayItOut.Components
{
    [Description("An image component allowing to include images in the layout.\n\nWhen loaded via `FormLoader`, the images are resolved via `BitmapLoader` instance.")]
    public class Image : Component
    {
        [Description("Image source. When parsed from XML, it can be a string containing image name or inline image base64 encoded in with prefix `base64:`.")]
        public Bitmap Src { get; set; }

        [Description("Specifies if and how image is scaled to the actual size of the component.")]
        public ImageScaling Scaling { get; set; }
        public RectangleF ImageLayout { get; private set; }
        public RectangleF ImageSourceRegion { get; private set; }

        protected override Size OnMeasure(Size size, IRendererContext context)
        {
            return Src?.Size ?? Size.Empty;
        }

        protected override void OnArrange()
        {
            if (Src == null)
            {
                ImageSourceRegion = ImageLayout = RectangleF.Empty;
            }
            else if (Scaling == ImageScaling.Fill)
            {
                ImageSourceRegion = new RectangleF(Point.Empty, Src.Size);
                ImageLayout = Layout;
            }
            else if (Scaling == ImageScaling.None)
            {
                var viewSize = new Size(Math.Min(Src.Width, Layout.Width), Math.Min(Src.Height, Layout.Height));
                ImageSourceRegion = ToRegion(new Rectangle(Point.Empty, Src.Size), viewSize);
                ImageLayout = ToRegion(Layout, viewSize);
            }
            else
            {
                ImageSourceRegion = new RectangleF(Point.Empty, Src.Size);
                var wRatio = Layout.Width / (float)Src.Width;
                var hRatio = Layout.Height / (float)Src.Height;
                var ratio = Math.Min(wRatio, hRatio);
                var viewSize = new SizeF(Src.Width * ratio, Src.Height * ratio);
                ImageLayout = ToRegion(Layout, viewSize);
            }
        }

        private RectangleF ToRegion(Rectangle rect, SizeF viewSize)
        {
            var shift = Alignment.GetShift(rect.Size - viewSize.Ceiling());
            var result = new RectangleF(rect.Location, viewSize);
            result.X += rect.Width > viewSize.Width ? shift.Width : -shift.Width;
            result.Y += rect.Height > viewSize.Height ? shift.Height : -shift.Height;
            return result;
        }
    }
}
