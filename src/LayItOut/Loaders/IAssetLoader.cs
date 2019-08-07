using System;
using System.Threading.Tasks;
using LayItOut.Attributes;

namespace LayItOut.Loaders
{
    public interface IAssetLoader : IDisposable
    {
        Task<AssetSource> LoadAsync(string src);
    }
}