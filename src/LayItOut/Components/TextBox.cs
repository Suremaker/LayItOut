using System;
using System.Collections.Generic;
using System.Drawing;
using LayItOut.Rendering;
using LayItOut.TextFormatting;

namespace LayItOut.Components
{
    public class TextBox : Component, IContainer, ITextComponent
    {
        public TextAlignment TextAlignment { get; set; }
        public TextLayout TextLayout { get; private set; }
        public TextMeasure TextMeasure { get; private set; }
        private readonly List<TextBlock> _blocks = new List<TextBlock>();
        private static readonly TextMeasurement TextMeasurement = new TextMeasurement();

        public void AddComponent(IComponent child)
        {
            if (!(child is Label label))
                throw new InvalidOperationException($"Only {nameof(Label)} and it's descendants are supported, got: {child.GetType().Name}");
            _blocks.AddRange(label.GetTextBlock().Normalize());
        }

        protected override Size OnMeasure(Size size, IRenderingContext context)
        {
            TextMeasure = TextMeasurement.Measure(context, size.Width, _blocks);
            return TextMeasure.Measure;
        }

        protected override void OnArrange()
        {
            TextLayout = TextMeasurement.LayOut(Layout.Width, TextAlignment, TextMeasure);
        }
    }
}
