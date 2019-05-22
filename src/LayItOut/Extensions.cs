using System.Drawing;

namespace LayItOut
{
    public static class Extensions
    {
        public static Size ApplyIfSet(this Size s, SizeUnit width, SizeUnit height) => new Size(width.ApplyIfSet(s.Width), height.ApplyIfSet(s.Height));
    }
}