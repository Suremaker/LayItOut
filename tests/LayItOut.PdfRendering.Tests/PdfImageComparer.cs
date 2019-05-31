using System;
using System.Drawing.Imaging;
using System.IO;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using PdfSharp.Pdf;
using Xunit;

namespace LayItOut.PdfRendering.Tests
{
    class PdfImageComparer
    {
        public static void ComparePdfs(string name, PdfDocument doc)
        {
            using (var pdfStream = new MemoryStream())
            using (var actualStream = new MemoryStream())
            using (var rasterizer = new GhostscriptRasterizer())
            {
                doc.Save(pdfStream);
                pdfStream.Seek(0, SeekOrigin.Begin);
                rasterizer.Open(pdfStream, new GhostscriptVersionInfo("gsdll64.dll"), false);
                var image = rasterizer.GetPage(72, 72, 1);
                image.Save(actualStream, ImageFormat.Bmp);
                var actual = actualStream.ToArray();

                var expected = File.ReadAllBytes($"{AppContext.BaseDirectory}\\expected\\{name}.bmp");
                if (!IsTheSame(actual, expected))
                {
                    var output = $"{AppContext.BaseDirectory}\\{name}.actual.bmp";
                    File.WriteAllBytes(output, actual);

                    var outputPdf = $"{AppContext.BaseDirectory}\\{name}.actual.pdf";
                    File.WriteAllBytes(outputPdf, pdfStream.ToArray());
                    Assert.True(false, $"Bitmap does not match: {output}");
                }
            }
        }

        private static bool IsTheSame(byte[] actual, byte[] expected)
        {
            if (actual.Length != expected.Length) return false;
            int diff = 0;
            for(int i=0;i<actual.Length;++i)
            {
                if (actual[i] != expected[i]) ++diff;
            }

            return diff * 100.0 / actual.Length < 1;
        }
    }
}