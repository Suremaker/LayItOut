using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LayItOut.BitmapRendering.Tests.Helpers;
using LayItOut.Loaders;
using Xunit;

namespace LayItOut.BitmapRendering.Tests
{
    public class PerformanceTests
    {
        private readonly byte[] _formInBytes = File.ReadAllBytes($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}form.xml");
        private readonly BitmapRenderer _renderer = new BitmapRenderer();
        private readonly FormLoader _loader = new FormLoader(new AssetLoader(x => true));

        [Fact]
        public async Task It_should_allow_concurrent_processing()
        {
            var bmps = await Task.WhenAll(Enumerable.Range(0, 100).Select(_ => Task.Run(Generate)));
            BitmapComparer.CompareBitmaps("form", bmps.First());
            BitmapComparer.CompareBitmaps("form", bmps.Last());
        }

        private async Task<Bitmap> Generate()
        {
            var form = await _loader.LoadForm(new MemoryStream(_formInBytes));
            return _renderer.Render(form);
        }
    }
}
