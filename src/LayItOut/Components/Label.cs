using System.ComponentModel;
using System.Drawing;
using LayItOut.Attributes;
using LayItOut.Rendering;
using LayItOut.TextFormatting;

namespace LayItOut.Components
{
    [Description("A label component allowing to include text in the layout.")]
    public class Label : Component, ITextComponent
    {
        private static readonly TextMeasurement TextMeasurement = new TextMeasurement();
        [Description("Font details.")]
        public FontInfo Font { get; set; }
        [Description("Color used to render text.")]
        public Color TextColor { get; set; } = Color.Black;
        [Description("Text to render.")]
        public string Text { get; set; }
        [Description("Specifies if whole text should render in-line or can break line between words.")]
        public bool Inline { get; set; }
        [Description("Specifies if that block of text should be rendered immediately after the last one.")]
        public bool TextContinuation { get; set; }
        [Description("Text alignment within component dimensions.")]
        public TextAlignment TextAlignment { get; set; }
        [Description("Line height multiplier, where 1.0 is a default size.")]
        public float LineHeight { get; set; } = 1;
        public TextLayout TextLayout { get; private set; }
        public TextMeasure TextMeasure { get; private set; }
        public override string ToString() => Text;

        public TextBlock GetTextBlock() => new TextBlock(Text, GetTextMetadata(), Inline, TextContinuation);

        protected override Size OnMeasure(Size size, IRendererContext context)
        {
            TextMeasure = TextMeasurement.Measure(context, size.Width, GetTextBlock());
            return TextMeasure.Measure;
        }

        protected override void OnArrange()
        {
            TextLayout = TextMeasurement.LayOut(Layout.Width, TextAlignment, TextMeasure);
        }

        protected virtual TextMetadata GetTextMetadata() => new TextMetadata(Font, TextColor, LineHeight);
    }
}
