using System.Drawing;
using LayItOut.Rendering;
using LayItOut.TextFormatting;

namespace LayItOut.Components
{
    public class Label : Component
    {
        public Font Font { get; set; }
        public Color FontColor { get; set; } = Color.Black;
        public string Text { get; set; }
        public bool Inline { get; set; }
        public TextLayout TextLayout { get; private set; }
        public override string ToString() => Text;

        protected override Size OnMeasure(Size size, IRenderingContext context)
        {
            var block = new TextBlock(Text, GetTextMetadata(), Inline);
            TextLayout = new TextMeasure(context).LayOut(size.Width, block);
            return TextLayout.Size;
        }

        protected virtual TextMetadata GetTextMetadata() => new TextMetadata(Font, FontColor);
    }
}
