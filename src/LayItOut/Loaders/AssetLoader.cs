using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using LayItOut.Attributes;

namespace LayItOut.Loaders
{
    public class AssetLoader : IAssetLoader
    {
        private ConcurrentDictionary<string, Lazy<Task<AssetSource>>> _cache = new ConcurrentDictionary<string, Lazy<Task<AssetSource>>>();
        private readonly Func<string, Task<byte[]>> _assetResolveFn;
        private readonly Func<string, bool> _shouldCache;
        public const string Base64Prefix = "base64:";

        public AssetLoader(Func<string, bool> shouldCache = null, Func<string, Task<byte[]>> assetResolveFn = null)
        {
            _shouldCache = shouldCache ?? DoNotCache;
            _assetResolveFn = assetResolveFn ?? LoadFromFile;
        }

        public async Task<AssetSource> LoadAsync(string src)
        {
            if (src.StartsWith(Base64Prefix))
            {
                using (var stream = new MemoryStream(Convert.FromBase64String(src.Substring(Base64Prefix.Length))))
                    return new AssetSource(null, stream.ToArray(), false);
            }

            if (_shouldCache(src))
                return await _cache.GetOrAdd(src, CreateLazyRead).Value;

            return await (_cache.TryGetValue(src, out var result) ? result.Value : LoadAsset(src, false));
        }

        private Lazy<Task<AssetSource>> CreateLazyRead(string src)
        {
            return new Lazy<Task<AssetSource>>(() => LoadAsset(src, true), LazyThreadSafetyMode.ExecutionAndPublication);
        }

        private async Task<AssetSource> LoadAsset(string src, bool cached) => new AssetSource(src, await _assetResolveFn(src), cached);

        private bool DoNotCache(string arg) => false;

        private Task<byte[]> LoadFromFile(string src)
        {
            //netstandard does not offer async read 
            return Task.FromResult(File.ReadAllBytes(src));
        }

        public void Dispose() => ClearCache();

        public void ClearCache()
        {
            Interlocked.Exchange(ref _cache, new ConcurrentDictionary<string, Lazy<Task<AssetSource>>>());
        }

        public void Cache(string src, byte[] content)
        {
            var lazy = new Lazy<Task<AssetSource>>(() => Task.FromResult(new AssetSource(src, content, true)));
            _cache.AddOrUpdate(src, lazy, (s, e) => lazy);
        }
    }
}