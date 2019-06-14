using System.Drawing;

namespace LayItOut.TextFormatting
{
    public interface ITextMetadata
    {
        Font Font { get; }
        Color Color { get; }
        string LinkHref { get; }
        float LineHeight { get; }
    }
}