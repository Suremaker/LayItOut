using System;
using System.Drawing;

namespace LayItOut.Loaders
{
    public interface IFontLoader : IDisposable
    {
        Font Parse(string value);
    }
}