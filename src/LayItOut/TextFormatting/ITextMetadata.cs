using System.Drawing;
using LayItOut.Attributes;

namespace LayItOut.TextFormatting
{
    public interface ITextMetadata
    {
        FontInfo Font { get; }
        Color Color { get; }
        string LinkHref { get; }
        float LineHeight { get; }
    }
}