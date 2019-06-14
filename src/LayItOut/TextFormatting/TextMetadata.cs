using System.Drawing;

namespace LayItOut.TextFormatting
{
    public class TextMetadata : ITextMetadata
    {
        public TextMetadata(Font font, Color color, float lineHeight)
        {
            Font = font;
            Color = color;
            LineHeight = lineHeight;
        }

        public Font Font { get; }
        public Color Color { get; }
        public string LinkHref { get; set; }
        public float LineHeight { get; }
    }
}