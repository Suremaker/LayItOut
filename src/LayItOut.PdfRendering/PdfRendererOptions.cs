using System;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering
{
    public class PdfRendererOptions
    {
        internal static readonly PdfRendererOptions Default = new PdfRendererOptions();
        public bool AdjustPageSize { get; set; }
        public Action<XGraphics> ConfigureGraphics { get; set; }
    }
}