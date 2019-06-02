using System;
using System.Drawing;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering
{
    //TODO: fix
    class RenderingContext : IRenderingContext
    {
        private readonly Graphics _graphics;

        public RenderingContext(Graphics graphics)
        {
            _graphics = graphics;
        }

        public SizeF MeasureText(string text, Font font)
        {
            throw new NotImplementedException();
        }

        public float GetSpaceWidth(Font font)
        {
            throw new NotImplementedException();
        }
    }
}