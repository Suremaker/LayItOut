using System.Collections.Concurrent;
using System.IO;
using LayItOut.Attributes;
using LayItOut.Caching;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering
{
    public class PdfBitmapCache : CachingResolver<AssetSource, XImage>
    {
        protected override XImage Create(AssetSource key) => XImage.FromStream(new MemoryStream(key.Content));
        protected override void OnDispose(ConcurrentDictionary<AssetSource, XImage> cache)
        {
            foreach (var image in cache.Values)
                image.Dispose();
        }
    }
}