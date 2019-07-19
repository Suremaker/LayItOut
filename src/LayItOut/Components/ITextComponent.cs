using LayItOut.Attributes;
using LayItOut.TextFormatting;

namespace LayItOut.Components
{
    public interface ITextComponent
    {
        TextAlignment TextAlignment { get; }
        TextLayout TextLayout { get; }
    }
}