using System.Drawing;
using LayItOut.Attributes;

namespace LayItOut.BitmapRendering
{
    public static class Conversions
    {
        public static FontStyle ToFontStyle(this FontInfoStyle s) => (FontStyle) (int) s;
    }
}