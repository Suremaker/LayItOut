using System;
using System.Drawing;

namespace LayItOut.Loaders
{
    public interface IBitmapLoader : IDisposable
    {
        Bitmap Load(string src);
    }
}