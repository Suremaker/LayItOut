using System.Drawing;
using LayItOut.Rendering;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering
{
    class RendererContext : IRenderingContext
    {
        private readonly XGraphics _graphics;

        public RendererContext(XGraphics graphics)
        {
            _graphics = graphics;
        }

        public SizeF MeasureText(string text, Font font)
        {
            var size = _graphics.MeasureString(text, font);
            return new SizeF((float) size.Width,(float) size.Height);
        }

        public float GetSpaceWidth(Font font)
        {
            //TODO: cache
            return (float) (_graphics.MeasureString("x x", font).Width - _graphics.MeasureString("xx", font).Width);
        }
    }
}