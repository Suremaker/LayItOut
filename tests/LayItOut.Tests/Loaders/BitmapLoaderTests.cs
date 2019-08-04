using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LayItOut.Attributes;
using LayItOut.Loaders;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Loaders
{
    public class AssetLoaderTests : IDisposable
    {
        private readonly string _tempFile = Path.GetTempFileName();

        [Fact]
        public async Task Loader_should_support_inline_assets()
        {
            var loader = new AssetLoader();
            var expected = "hello";
            var base64 = StoreAsBase64(expected);
            var actual = await loader.LoadAsync($"{AssetLoader.Base64Prefix}{base64}");
            AssertStringAsset(expected, actual);
        }

        [Fact]
        public async Task Loader_should_load_files_by_default()
        {
            var loader = new AssetLoader();
            var expected = "hello";
            File.WriteAllText(_tempFile, expected);
            var actual = await loader.LoadAsync(_tempFile);
            AssertStringAsset(expected, actual);
        }

        [Fact]
        public async Task Loader_should_load_files_but_do_not_cache_them_by_default()
        {
            var loader = new AssetLoader();
            var expected = "hi!";
            File.WriteAllText(_tempFile, expected);
            var actual = await loader.LoadAsync(_tempFile);
            AssertStringAsset(expected, actual);

            File.Delete(_tempFile);
            await Assert.ThrowsAsync<FileNotFoundException>(() => loader.LoadAsync(_tempFile));
        }

        [Fact]
        public async Task Loader_should_cache_images_if_specified()
        {
            var loader = new AssetLoader(src => true);
            var expected = "hello";
            File.WriteAllText(_tempFile, expected);

            var actual = await loader.LoadAsync(_tempFile);
            AssertStringAsset(expected, actual);
            File.Delete(_tempFile);

            var actual2 = await loader.LoadAsync(_tempFile);
            AssertStringAsset(expected, actual2);
        }

        [Fact]
        public async Task Loader_should_allow_custom_resolve_method()
        {

            var expected = new byte[] { 1, 2, 3 };
            var loader = new AssetLoader(assetResolveFn: _ => Task.FromResult(expected));
            var actual = await loader.LoadAsync("anything");
            actual.Content.ShouldBeSameAs(expected);
        }

        [Fact]
        public async Task Loader_should_allow_prepopulate_cache()
        {
            var expected1 = "hello";
            var expected2 = "hi!";

            var loader = new AssetLoader();
            loader.Cache("red", Encoding.UTF8.GetBytes(expected1));
            loader.Cache("blue", Encoding.UTF8.GetBytes(expected2));
            AssertStringAsset(expected1, await loader.LoadAsync("red"));
            AssertStringAsset(expected2, await loader.LoadAsync("blue"));
        }

        [Fact]
        public async Task ClearCache_should_clear_cache()
        {
            var expected = "hello";
            var loader = new AssetLoader(assetResolveFn: _ => throw new IOException());
            loader.Cache("red", Encoding.UTF8.GetBytes(expected));
            AssertStringAsset(expected, await loader.LoadAsync("red"));
            loader.ClearCache();
            await Assert.ThrowsAsync<IOException>(() => loader.LoadAsync("red"));
        }

        private void AssertStringAsset(string expected, AssetSource actual)
        {
            Encoding.UTF8.GetString(actual.Content).ShouldBe(expected);
        }

        private string StoreAsBase64(string text) => Convert.ToBase64String(Encoding.UTF8.GetBytes(text));

        public void Dispose()
        {
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }
    }
}