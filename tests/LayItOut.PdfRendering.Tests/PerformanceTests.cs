using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LayItOut.Loaders;
using LayItOut.PdfRendering.Tests.Helpers;
using PdfSharp.Pdf;
using Xunit;

namespace LayItOut.PdfRendering.Tests
{
    public class PerformanceTests
    {
        private readonly byte[] _formInBytes = File.ReadAllBytes($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}form.xml");
        private readonly PdfRenderer _renderer = new PdfRenderer();
        private readonly FormLoader _loader;

        public PerformanceTests()
        {
            var parser = new FontParser();
            parser.AddFont($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}ahronbd.ttf");
            _loader = new FormLoader(parser);
        }

        [Fact]
        public async Task It_should_allow_concurrent_processing()
        {
            var pdfs = await Task.WhenAll(Enumerable.Range(0, 100).Select(_ => Task.Run(Generate)));

            PdfImageComparer.ComparePdfs("form", pdfs.First());
            PdfImageComparer.ComparePdfs("form", pdfs.Last());
        }
        private byte[] Generate()
        {
            var form = _loader.Load(new MemoryStream(_formInBytes));
            var pdf = new PdfDocument();
            _renderer.Render(form, pdf.AddPage(), new PdfRendererOptions { AdjustPageSize = true });
            _renderer.Render(form, pdf.AddPage(), new PdfRendererOptions { ConfigureGraphics = x => x.ScaleTransform(2, 2) });
            using (var mem = new MemoryStream())
            {
                pdf.Save(mem);
                return mem.ToArray();
            }
        }
    }
}
