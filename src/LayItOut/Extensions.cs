using System;
using System.Drawing;

namespace LayItOut
{
    public static class Extensions
    {
        public static Size ApplyIfSet(this Size s, SizeUnit width, SizeUnit height) => new Size(width.ApplyIfSet(s.Width), height.ApplyIfSet(s.Height));
        public static Size Intersect(this Size x, Size y) => new Size(Math.Min(x.Width, y.Width), Math.Min(x.Height, y.Height));
    }
}