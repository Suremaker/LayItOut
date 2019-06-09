using System.Drawing;

namespace LayItOut.TextFormatting
{
    public class TextMetadata : ITextMetadata
    {
        public TextMetadata(Font font, Color color)
        {
            Font = font;
            Color = color;
        }

        public Font Font { get; }
        public Color Color { get; }
        public string LinkHref { get; set; }
    }
}