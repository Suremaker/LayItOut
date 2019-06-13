using System.Drawing;
using LayItOut.Rendering;
using LayItOut.TextFormatting;

namespace LayItOut.Components
{
    public class Label : Component, ITextComponent
    {
        public Font Font { get; set; }
        public Color FontColor { get; set; } = Color.Black;
        public string Text { get; set; }
        public bool Inline { get; set; }
        public TextAlignment TextAlignment { get; set; }
        public TextLayout TextLayout { get; private set; }
        public override string ToString() => Text;

        public TextBlock GetTextBlock() => new TextBlock(Text, GetTextMetadata(), Inline);

        protected override Size OnMeasure(Size size, IRenderingContext context)
        {
            TextLayout = new TextMeasure(context).LayOut(size.Width, TextAlignment, GetTextBlock());
            return TextLayout.Size;
        }

        protected virtual TextMetadata GetTextMetadata() => new TextMetadata(Font, FontColor);
    }
}
