using System;
using System.Drawing;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering
{
    class RenderingContext : IRenderingContext
    {
        private readonly Graphics _graphics;

        public RenderingContext(Graphics graphics)
        {
            _graphics = graphics;
        }

        public SizeF MeasureText(string text, Font font)
        {
            var state = _graphics.TextRenderingHint;
            try
            {
                _graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                return _graphics.MeasureString(text, font, new SizeF(int.MaxValue, int.MaxValue), StringFormat.GenericTypographic);
            }
            finally
            {
                _graphics.TextRenderingHint = state;
            }
        }

        public float GetSpaceWidth(Font font)
        {
            return MeasureText("x x", font).Width - MeasureText("xx", font).Width;
        }
    }
}