using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Xunit;

namespace LayItOut.BitmapRendering.Tests.Helpers
{
    static class BitmapComparer
    {
        public static void CompareBitmaps(string name, Bitmap bmp)
        {
            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, ImageFormat.Bmp);
                var actual = stream.ToArray();
                var output = $"{AppContext.BaseDirectory}\\{name}.actual.bmp";
                File.WriteAllBytes(output, actual);
                var expected = File.ReadAllBytes($"{AppContext.BaseDirectory}\\expected\\{name}.bmp");
                if (!actual.SequenceEqual(expected))
                    Assert.True(false, $"Bitmap does not match: {output}");
            }
        }
    }
}