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
            {
                doc.Save(pdfStream);
                ComparePdfs(name,pdfStream.ToArray());
            }
        }
        public static void ComparePdfs(string name, byte[] pdfBytes)
        {
            using (var actualStream = new MemoryStream())
            {
                var image = GetImage(pdfBytes);
                image.Save(actualStream, ImageFormat.Bmp);
                var actual = actualStream.ToArray();

                var output = $"{AppContext.BaseDirectory}\\{name}.actual.bmp";
                File.WriteAllBytes(output, actual);

                var outputPdf = $"{AppContext.BaseDirectory}\\{name}.actual.pdf";
                File.WriteAllBytes(outputPdf, pdfBytes);

                var expected = File.ReadAllBytes($"{AppContext.BaseDirectory}\\expected\\{name}.bmp");

                if (!IsTheSame(actual, expected))
                    Assert.True(false, $"Bitmap does not match: {output}");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static Image GetImage(byte[] pdfBytes)
        {
            using (var rasterizer = new GhostscriptRasterizer())
            {
                rasterizer.Open(new MemoryStream(pdfBytes), new GhostscriptVersionInfo("gsdll64.dll"), false);
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