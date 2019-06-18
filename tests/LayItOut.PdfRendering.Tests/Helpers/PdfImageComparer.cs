using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using PdfSharp.Pdf;
using Xunit;

namespace LayItOut.PdfRendering.Tests.Helpers
{
    class PdfImageComparer
    {
        public static void ComparePdfs(string name, PdfDocument doc)
        {
            using (var pdfStream = new MemoryStream())
            using (var actualStream = new MemoryStream())
            {
                doc.Save(pdfStream);
                pdfStream.Seek(0, SeekOrigin.Begin);
                var image = GetImage(pdfStream);
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static Image GetImage(MemoryStream pdfStream)
        {
            using (var rasterizer = new GhostscriptRasterizer())
            {
                rasterizer.Open(pdfStream, new GhostscriptVersionInfo("gsdll64.dll"), false);
                return rasterizer.GetPage(72, 72, 1);
            }
        }

        private static bool IsTheSame(byte[] actual, byte[] expected)
        {
            if (actual.Length != expected.Length) return false;
            int diff = 0;
            for (int i = 0; i < actual.Length; ++i)
            {
                if (actual[i] != expected[i]) ++diff;
            }

            var actualDiff = diff * 100.0 / actual.Length;
            return actualDiff < 1.5;
        }
    }
}