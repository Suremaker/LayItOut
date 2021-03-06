﻿using System.Collections.Concurrent;
using System.Drawing;
using LayItOut.Attributes;
using LayItOut.Rendering;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering
{
    public class PdfRendererContext : IRendererContext
    {
        private readonly PdfBitmapCache _localBitmapCache;
        private readonly ConcurrentDictionary<FontInfo, float> _spaceSizes = new ConcurrentDictionary<FontInfo, float>();
        public XGraphics Graphics { get; }
        public PdfFontResolver FontResolver { get; }

        public PdfRendererContext(XGraphics graphics, PdfFontResolver fontResolver, PdfBitmapCache localBitmapCache)
        {
            _localBitmapCache = localBitmapCache;
            Graphics = graphics;
            FontResolver = fontResolver;
        }

        public SizeF MeasureText(string text, FontInfo font)
        {
            var size = Graphics.MeasureString(text, FontResolver.Resolve(font));
            return new SizeF((float)size.Width, (float)size.Height);
        }

        public float GetSpaceWidth(FontInfo font)
        {
            return _spaceSizes.GetOrAdd(font, CalculateSpaceSize);
        }

        public Size MeasureBitmap(AssetSource bitmap)
        {
            var bmp = GetBitmap(bitmap);
            return new Size(bmp.PixelWidth, bmp.PixelHeight);
        }

        public XImage GetBitmap(AssetSource bitmap)
        {
            return _localBitmapCache.Resolve(bitmap);
        }

        private float CalculateSpaceSize(FontInfo font)
        {
            var xfont = FontResolver.Resolve(font);
            return (float)(Graphics.MeasureString("x x", xfont).Width - Graphics.MeasureString("xx", xfont).Width);
        }
    }
}