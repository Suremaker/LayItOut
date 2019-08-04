using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using LayItOut.Attributes;
using LayItOut.Caching;
using LayItOut.Rendering;

namespace LayItOut.Tests.TestHelpers
{
    public class TestRendererContext : IRendererContext
    {
        public static readonly TestRendererContext Instance = new TestRendererContext();
        private BitmapCache _bitmapResolver = new BitmapCache();

        private TestRendererContext() { }

        public SizeF MeasureText(string text, FontInfo font)
        {
            return font.Size == 0
                ? Size.Empty
                : new SizeF(text.Length, GetHeight(font));
        }

        public float GetSpaceWidth(FontInfo font)
        {
            return 1;
        }

        public Size MeasureBitmap(AssetSource bitmap)
        {
            return _bitmapResolver.Resolve(bitmap).Size;
        }

        /// <summary>
        /// Some simulated height based on font size - purely for testing purposes.
        /// </summary>
        public float GetHeight(FontInfo font)
        {
            return font.Size * 1.2f;
        }

        private class BitmapCache : CachingResolver<AssetSource, Bitmap>
        {
            protected override void OnDispose(ConcurrentDictionary<AssetSource, Bitmap> cache)
            {
                foreach (var image in cache.Values)
                    image.Dispose();
            }

            protected override Bitmap Create(AssetSource src) => new Bitmap(new MemoryStream(src.Content));
        }
    }
}
