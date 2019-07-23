using System.Drawing;
using LayItOut.Attributes;

namespace LayItOut.TextFormatting
{
    public class TextMetadata : ITextMetadata
    {
        public TextMetadata(FontInfo font, Color color, float lineHeight)
        {
            Font = font;
            Color = color;
            LineHeight = lineHeight;
        }

        public FontInfo Font { get; }
        public Color Color { get; }
        public string LinkHref { get; set; }
        public float LineHeight { get; }
    }
}