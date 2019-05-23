using System;
using System.Drawing;

namespace LayItOut
{
    public static class Extensions
    {
        public static Size ApplyIfSet(this Size s, SizeUnit width, SizeUnit height) => new Size(width.ApplyIfSet(s.Width), height.ApplyIfSet(s.Height));
        public static Size Intersect(this Size x, Size y) => new Size(Math.Min(x.Width, y.Width), Math.Min(x.Height, y.Height));

        public static Size GetShift(this HorizontalAlignment alignment, int width)
        {
            if (alignment == HorizontalAlignment.Center)
                width /= 2;
            else if (alignment == HorizontalAlignment.Left)
                width = 0;
            return new Size(Math.Max(0, width), 0);
        }

        public static Size GetShift(this VerticalAlignment alignment, int height)
        {
            if (alignment == VerticalAlignment.Center)
                height /= 2;
            else if (alignment == VerticalAlignment.Top)
                height = 0;
            return new Size(0, Math.Max(0, height));
        }

        public static Size GetShift(this Alignment alignment, Size size) => alignment.Horizontal.GetShift(size.Width) + alignment.Vertical.GetShift(size.Height);
    }
}