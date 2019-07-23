using System;
using System.Drawing;

namespace LayItOut.BitmapRendering
{
    public class BitmapRendererOptions
    {
        internal static readonly BitmapRendererOptions Default = new BitmapRendererOptions();
        public Action<Graphics> ConfigureGraphics { get; set; }
    }
}