using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Threading;

namespace LayItOut.Loaders
{
    public class BitmapLoader : IBitmapLoader
    {
        private ConcurrentDictionary<string, Bitmap> _cache = new ConcurrentDictionary<string, Bitmap>();
        private readonly Func<string, Bitmap> _bitmapResolveFn;
        private readonly Func<string, bool> _shouldCache;
        public const string Base64Prefix = "base64:";

        public BitmapLoader(Func<string, bool> shouldCache = null, Func<string, Bitmap> bitmapResolveFn = null)
        {
            _shouldCache = shouldCache ?? DoNotCache;
            _bitmapResolveFn = bitmapResolveFn ?? LoadFromFile;
        }

        public Bitmap Load(string src)
        {
            if (src.StartsWith(Base64Prefix))
            {
                using (var stream = new MemoryStream(Convert.FromBase64String(src.Substring(Base64Prefix.Length))))
                    return new Bitmap(stream);
            }

            if (_shouldCache(src))
                return Clone(_cache.GetOrAdd(src, _bitmapResolveFn));

            return _cache.TryGetValue(src, out var result) ? Clone(result) : _bitmapResolveFn(src);
        }

        private Bitmap Clone(Bitmap cached)
        {
            lock (cached)
                return (Bitmap)cached.Clone();
        }

        private bool DoNotCache(string arg) => false;

        private Bitmap LoadFromFile(string src)
        {
            using (var stream = File.OpenRead(src))
                return new Bitmap(stream);
        }

        public void Dispose() => ClearCache();

        public void ClearCache()
        {
            var old = Interlocked.Exchange(ref _cache, new ConcurrentDictionary<string, Bitmap>());

            foreach (var bmp in old.Values)
                bmp.Dispose();
        }

        public void Cache(string src, Bitmap value)
        {
            _cache.AddOrUpdate(src, value, (s, e) => value);
        }
    }
}