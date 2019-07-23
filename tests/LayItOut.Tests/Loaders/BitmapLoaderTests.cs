using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using LayItOut.Loaders;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Loaders
{
    public class BitmapLoaderTests : IDisposable
    {
        private readonly string _tempFile = Path.GetTempFileName();

        [Fact]
        public void Loader_should_support_inline_bitmaps()
        {
            var loader = new BitmapLoader();
            var expected = MakeBmp();
            var base64 = StoreAsBase64(expected);
            var actual = loader.Load($"{BitmapLoader.Base64Prefix}{base64}");
            AssertBitmap(expected, actual);
        }

        [Fact]
        public void Loader_should_load_files_by_default()
        {
            var loader = new BitmapLoader();
            var expected = MakeBmp();
            expected.Save(_tempFile, ImageFormat.Bmp);
            var actual = loader.Load(_tempFile);
            AssertBitmap(expected, actual);
        }

        [Fact]
        public void Loader_should_load_files_but_do_not_cache_them_by_default()
        {
            var loader = new BitmapLoader();
            var expected = MakeBmp();
            expected.Save(_tempFile, ImageFormat.Bmp);
            var actual = loader.Load(_tempFile);
            AssertBitmap(expected, actual);

            File.Delete(_tempFile);
            Assert.Throws<FileNotFoundException>(() => loader.Load(_tempFile));
        }

        [Fact]
        public void Loader_should_cache_images_if_specified()
        {
            var loader = new BitmapLoader(src => true);
            var expected = MakeBmp();
            expected.Save(_tempFile, ImageFormat.Bmp);

            var actual = loader.Load(_tempFile);
            AssertBitmap(expected, actual);
            File.Delete(_tempFile);

            var actual2 = loader.Load(_tempFile);
            AssertBitmap(actual,actual2);
        }

        [Fact]
        public void Loader_should_allow_custom_resolve_method()
        {
            var expected = MakeBmp();
            var loader = new BitmapLoader(bitmapResolveFn: _ => expected);
            loader.Load("anything").ShouldBeSameAs(expected);
        }

        [Fact]
        public void Loader_should_allow_prepopulate_cache()
        {
            var expected1 = MakeBmp();
            var expected2 = MakeBmp(Brushes.Blue);

            var loader = new BitmapLoader();
            loader.Cache("red", expected1);
            loader.Cache("blue", expected2);
            AssertBitmap(expected1,loader.Load("red"));
            AssertBitmap(expected2,loader.Load("blue"));
        }

        [Fact]
        public void ClearCache_should_clear_cache()
        {
            var expected = MakeBmp();
            var loader = new BitmapLoader(bitmapResolveFn: _ => throw new IOException());
            loader.Cache("red", expected);
            AssertBitmap(expected,loader.Load("red"));
            loader.ClearCache();
            Assert.Throws<IOException>(() => loader.Load("red"));
        }

        private void AssertBitmap(Bitmap expected, Bitmap actual)
        {
            actual.Size.ShouldBe(expected.Size);
            for (int x = 0; x < expected.Width; ++x)
                for (int y = 0; y < expected.Height; ++y)
                    actual.GetPixel(x, y).ShouldBe(expected.GetPixel(x, y), $"pixels at {x},{y} does not match");
        }

        private string StoreAsBase64(Bitmap bmp)
        {
            using (var memory = new MemoryStream())
            {
                bmp.Save(memory, ImageFormat.Png);
                return Convert.ToBase64String(memory.ToArray());
            }
        }

        private static Bitmap MakeBmp(Brush bg = null)
        {
            var bmp = new Bitmap(10, 10);
            using (var g = Graphics.FromImage(bmp))
            {
                g.FillRectangle(bg ?? Brushes.Red, 0, 0, 10, 10);
                g.FillRectangle(Brushes.Yellow, 2, 2, 6, 6);
            }

            return bmp;
        }

        public void Dispose()
        {
            if (File.Exists(_tempFile))
                File.Delete(_tempFile);
        }
    }
}