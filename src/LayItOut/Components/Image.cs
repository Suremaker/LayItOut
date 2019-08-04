using System;
using System.ComponentModel;
using System.Drawing;
using LayItOut.Attributes;
using LayItOut.Rendering;

namespace LayItOut.Components
{
    [Description("An image component allowing to include images in the layout.\n\nWhen loaded via `FormLoader`, the images are resolved via `AssetLoader` instance.")]
    public class Image : Component
    {
        [Description("Image source. When parsed from XML, it can be a string containing image name or inline image base64 encoded in with prefix `base64:`.")]
        public AssetSource Src { get; set; }

        [Description("Specifies if and how image is scaled to the actual size of the component.")]
        public ImageScaling Scaling { get; set; }
        public RectangleF ImageLayout { get; private set; }
        public RectangleF ImageSourceRegion { get; private set; }
        public Size MeasuredImageSize { get; private set; }

        protected override Size OnMeasure(Size size, IRendererContext context)
        {
            return MeasuredImageSize = !Src.IsNone ? context.MeasureBitmap(Src) : Size.Empty;
        }

        protected override void OnArrange()
        {
            if (Src.IsNone)
            {
                ImageSourceRegion = ImageLayout = RectangleF.Empty;
            }
            else if (Scaling == ImageScaling.Fill)
            {
                ImageSourceRegion = new RectangleF(Point.Empty, MeasuredImageSize);
                ImageLayout = Layout;
            }
            else if (Scaling == ImageScaling.None)
            {
                var viewSize = new Size(Math.Min(MeasuredImageSize.Width, Layout.Width), Math.Min(MeasuredImageSize.Height, Layout.Height));
                ImageSourceRegion = ToRegion(new Rectangle(Point.Empty, MeasuredImageSize), viewSize);
                ImageLayout = ToRegion(Layout, viewSize);
            }
            else
            {
                ImageSourceRegion = new RectangleF(Point.Empty, MeasuredImageSize);
                var wRatio = Layout.Width / (float)MeasuredImageSize.Width;
                var hRatio = Layout.Height / (float)MeasuredImageSize.Height;
                var ratio = Math.Min(wRatio, hRatio);
                var viewSize = new SizeF(MeasuredImageSize.Width * ratio, MeasuredImageSize.Height * ratio);
                ImageLayout = ToRegion(Layout, viewSize);
            }
        }

        private RectangleF ToRegion(Rectangle rect, SizeF viewSize)
        {
            var shift = Alignment.GetShift(rect.Size - viewSize);
            var result = new RectangleF(rect.Location, viewSize);
            result.X += rect.Width > viewSize.Width ? shift.Width : -shift.Width;
            result.Y += rect.Height > viewSize.Height ? shift.Height : -shift.Height;
            return result;
        }
    }
}
