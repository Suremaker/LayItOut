using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Text;
using LayItOut.Attributes;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering
{
    public class RendererContext : IRendererContext
    {
        private readonly ConcurrentDictionary<FontInfo, float> _spaceSizes = new ConcurrentDictionary<FontInfo, float>();
        public FontResolver FontResolver { get; }
        public Graphics Graphics { get; }

        public RendererContext(Graphics graphics, FontResolver fontResolver)
        {
            FontResolver = fontResolver;
            Graphics = graphics;
        }

        public SizeF MeasureText(string text, FontInfo font)
        {
            var state = Graphics.TextRenderingHint;
            try
            {
                Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                return Graphics.MeasureString(text, FontResolver.Resolve(font), new SizeF(int.MaxValue, int.MaxValue), StringFormat.GenericTypographic);
            }
            finally
            {
                Graphics.TextRenderingHint = state;
            }
        }

        public float GetSpaceWidth(FontInfo font)
        {
            return _spaceSizes.GetOrAdd(font, CalculateSpaceSize);
        }

        private float CalculateSpaceSize(FontInfo font)
        {
            var xfont = FontResolver.Resolve(font);
            return Graphics.MeasureString("x x", xfont).Width - Graphics.MeasureString("xx", xfont).Width;
        }
    }
}