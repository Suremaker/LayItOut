using System;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace LayItOut.Tests.TestHelpers
{
    internal static class RectParser
    {
        public static Size ToSize(string text)
        {
            var parts = text.Split('x').Select(Int32.Parse).ToArray();
            return new Size(parts[0], parts[1]);
        }

        private static SizeF ToSizeF(string text)
        {
            var parts = text.Split('x').Select(s => Single.Parse(s, CultureInfo.InvariantCulture)).ToArray();
            return new SizeF(parts[0], parts[1]);
        }

        private static Point ToPoint(string text)
        {
            var parts = text.Split(':').Select(Int32.Parse).ToArray();
            return new Point(parts[0], parts[1]);
        }

        private static PointF ToPointF(string text)
        {
            var parts = text.Split(':').Select(s => Single.Parse(s, CultureInfo.InvariantCulture)).ToArray();
            return new PointF(parts[0], parts[1]);
        }

        public static Rectangle ToRect(string text)
        {
            var parts = text.Split('|');
            return new Rectangle(ToPoint(parts[0]), ToSize(parts[1]));
        }

        public static RectangleF ToRectF(string text)
        {
            var parts = text.Split('|');
            return new RectangleF(ToPointF(parts[0]), ToSizeF(parts[1]));
        }
    }
}