using System.Collections.Concurrent;
using System.Drawing;
using LayItOut.Attributes;

namespace LayItOut.BitmapRendering
{
    public class BitmapFontResolver
    {
        private readonly ConcurrentDictionary<FontInfo, Font> _cache = new ConcurrentDictionary<FontInfo, Font>();

        public Font Resolve(FontInfo font) => _cache.GetOrAdd(font, i => new Font(new FontFamily(i.Family), i.Size, i.Style.ToFontStyle(), GraphicsUnit.World));
    }
}