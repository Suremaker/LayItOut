using System;
using System.Collections.Generic;
using System.Drawing;
using LayItOut.Attributes;

namespace LayItOut
{
    public static class Extensions
    {
        public static Size ApplyIfSet(this Size s, SizeUnit width, SizeUnit height) => new Size(width.AbsoluteOrDefault(s.Width), height.AbsoluteOrDefault(s.Height));
        public static Size Intersect(this Size x, Size y) => new Size(Math.Min(x.Width, y.Width), Math.Min(x.Height, y.Height));
        public static Size Union(this Size x, Size y) => new Size(Math.Max(x.Width, y.Width), Math.Max(x.Height, y.Height));

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

        public static Rectangle ShrinkBy(this Rectangle rect, Spacer spacer)
        {
            var l = spacer.Left.GetValue(rect.Width);
            var r = spacer.Right.GetValue(rect.Width);
            var t = spacer.Top.GetValue(rect.Height);
            var b = spacer.Bottom.GetValue(rect.Height);
            var size = new Size(rect.Width - l - r, rect.Height - t - b);
            if (size.Width < 0 || size.Height < 0)
                return new Rectangle(rect.Location + new Size(Math.Min(l, rect.Width), Math.Min(t, rect.Height)), Size.Empty);
            return new Rectangle(rect.Left + l, rect.Top + t, size.Width, size.Height);
        }

        internal static T GetNext<T>(this IEnumerator<T> enumerator) => enumerator.MoveNext() ? enumerator.Current : throw new IndexOutOfRangeException();
    }
}