﻿using System;
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
        private readonly FormLoader _loader = new FormLoader(new AssetLoader(x => true));

        [Fact]
        public async Task It_should_allow_concurrent_processing()
        {
            var pdfs = await Task.WhenAll(Enumerable.Range(0, 100).Select(_ => Task.Run(Generate)));

            PdfImageComparer.ComparePdfs("form", pdfs.First());
            PdfImageComparer.ComparePdfs("form", pdfs.Last());
        }

        private async Task<byte[]> Generate()
        {
            var form = await _loader.LoadForm(new MemoryStream(_formInBytes));
            var pdf = new PdfDocument();
            _renderer.Render(form, pdf.AddPage(), new PdfRendererOptions { AdjustPageSize = true });
            _renderer.Render(form, pdf.AddPage(), new PdfRendererOptions { ConfigureGraphics = x => x.ScaleTransform(0.5, 0.5) });
            using (var mem = new MemoryStream())
            {
                pdf.Save(mem);
                return mem.ToArray();
            }
        }
    }
}
