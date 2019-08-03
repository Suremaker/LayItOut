using System.ComponentModel;

namespace LayItOut.Attributes
{
    [Description("Image scaling options.")]
    public enum ImageScaling : byte
    {
        None = 0,
        Fill = 1,
        Uniform = 2
    }
}