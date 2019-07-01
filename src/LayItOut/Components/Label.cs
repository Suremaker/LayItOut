using System.Drawing;
using LayItOut.Rendering;
using LayItOut.TextFormatting;

namespace LayItOut.Components
{
    public class Label : Component, ITextComponent
    {
        private static readonly TextMeasurement TextMeasurement = new TextMeasurement();
        public FontInfo Font { get; set; }
        public Color FontColor { get; set; } = Color.Black;
        public string Text { get; set; }
        public bool Inline { get; set; }
        public TextAlignment TextAlignment { get; set; }
        public TextLayout TextLayout { get; private set; }
        public TextMeasure TextMeasure { get; private set; }
        public float LineHeight { get; set; } = 1;
        public override string ToString() => Text;

        public TextBlock GetTextBlock() => new TextBlock(Text, GetTextMetadata(), Inline);

        protected override Size OnMeasure(Size size, IRendererContext context)
        {
            TextMeasure = TextMeasurement.Measure(context, size.Width, GetTextBlock());
            return TextMeasure.Measure;
        }

        protected override void OnArrange()
        {
            TextLayout = TextMeasurement.LayOut(Layout.Width, TextAlignment, TextMeasure);
        }

        protected virtual TextMetadata GetTextMetadata() => new TextMetadata(Font, FontColor, LineHeight);
    }
}
