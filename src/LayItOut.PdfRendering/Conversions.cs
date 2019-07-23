using System.Drawing;
using LayItOut.Attributes;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering
{
    public static class Conversions
    {
        public static int ToLayout(this XUnit unit) => (int)unit.Point;
        public static XRect ToXRect(this Rectangle r) => new XRect(r.Location.ToXPoint(), r.Size.ToXSize());
        public static XRect ToXRect(this RectangleF r) => new XRect(r.Location.ToXPoint(), r.Size.ToXSize());
        public static XPoint ToXPoint(this Point p) => new XPoint(p.X, p.Y);
        public static XPoint ToXPoint(this PointF p) => new XPoint(p.X, p.Y);
        public static XSize ToXSize(this Size s) => new XSize(s.Width, s.Height);
        public static XSize ToXSize(this SizeF s) => new XSize(s.Width, s.Height);
        public static XColor ToXColor(this Color c) => XColor.FromArgb(c.ToArgb());
        public static double ToXWidth(this SizeUnit unit) => unit.AbsoluteOrDefault();
        public static XFontStyle ToXFontStyle(this FontInfoStyle s) => (XFontStyle) (int) s;
    }
}