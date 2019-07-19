using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using LayItOut.Attributes;
using LayItOut.Rendering;
using LayItOut.TextFormatting;

namespace LayItOut.Components
{
    [Description("Text box container allowing to lay-out text composed of various size/font/color text and link blocks.\n\nThe container can hold multiple instances of **Label** or **Link** components.")]
    public class TextBox : Component, IContainer, ITextComponent
    {
        [Description("Text alignment used to align all the rendered text. Please note that *TextAlignment* property of child components will be **ignored**.")]
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

        protected override Size OnMeasure(Size size, IRendererContext context)
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
