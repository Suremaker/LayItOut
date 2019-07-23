using System.Collections.Concurrent;
using LayItOut.Attributes;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering
{
    public class PdfFontResolver
    {
        private readonly ConcurrentDictionary<FontInfo, XFont> _cache = new ConcurrentDictionary<FontInfo, XFont>();

        public XFont Resolve(FontInfo font) => _cache.GetOrAdd(font, i => new XFont(i.Family, i.Size, i.Style.ToXFontStyle()));
    }
}