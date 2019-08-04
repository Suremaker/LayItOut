using System;
using System.Collections.Concurrent;
using System.Threading;

namespace LayItOut.Caching
{
    public abstract class CachingResolver<TKey, TValue> : IDisposable
    {
        private ConcurrentDictionary<TKey, TValue> _cache = new ConcurrentDictionary<TKey, TValue>();

        public void Clear()
        {
            var old = Interlocked.Exchange(ref _cache, new ConcurrentDictionary<TKey, TValue>());
            OnDispose(old);
        }

        protected virtual void OnDispose(ConcurrentDictionary<TKey, TValue> cache) { }

        public TValue Resolve(TKey key) => _cache.GetOrAdd(key, Create);

        protected abstract TValue Create(TKey key);

        public void Dispose() => Clear();
    }
}
