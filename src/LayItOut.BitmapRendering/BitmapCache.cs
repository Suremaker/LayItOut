using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using LayItOut.Attributes;
using LayItOut.Caching;

namespace LayItOut.BitmapRendering
{
    public class BitmapCache : CachingResolver<AssetSource, Bitmap>
    {
        protected override void OnDispose(ConcurrentDictionary<AssetSource, Bitmap> cache)
        {
            foreach (var image in cache.Values)
                image.Dispose();
        }

        protected override Bitmap Create(AssetSource src) => new Bitmap(new MemoryStream(src.Content));
    }
}