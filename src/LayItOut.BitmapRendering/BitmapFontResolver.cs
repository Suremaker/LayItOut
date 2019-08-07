using System.Collections.Concurrent;
using System.Drawing;
using LayItOut.Attributes;
using LayItOut.Caching;

namespace LayItOut.BitmapRendering
{
    public class BitmapFontResolver : CachingResolver<FontInfo, Font>
    {
        protected override Font Create(FontInfo i) => new Font(new FontFamily(i.Family), i.Size, i.Style.ToFontStyle(), GraphicsUnit.World);
        protected override void OnDispose(ConcurrentDictionary<FontInfo, Font> cache)
        {
            foreach (var font in cache.Values)
                font.Dispose();
        }
    }
}