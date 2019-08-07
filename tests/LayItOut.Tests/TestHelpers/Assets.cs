using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using LayItOut.Attributes;

namespace LayItOut.Tests.TestHelpers
{
    public static class Assets
    {
        public static AssetSource ToAssetSource(this Bitmap bmp)
        {
            var mem = new MemoryStream();
            bmp.Save(mem, ImageFormat.Bmp);
            return new AssetSource(null, mem.ToArray(), false);
        }
    }
}
