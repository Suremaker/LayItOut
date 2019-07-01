using System;
using System.Drawing;
using LayItOut.Components;
using LayItOut.Tests.TestHelpers;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Components
{
    public class LabelTests
    {
        [Theory]
        [InlineData(typeof(Label))]
        [InlineData(typeof(Link))]
        public void Measure_should_use_rendering_context(Type labelType)
        {
            var text = "foo bar";
            var box = new Size(20, 30);
            var font = new FontInfo("Test", 14);
            var label = (Label)Activator.CreateInstance(labelType);
            label.Text = text;
            label.Font = font;

            label.Measure(box, TestRendererContext.Instance);
            label.DesiredSize.ShouldBe(new Size(text.Length, (int)Math.Ceiling(TestRendererContext.Instance.GetHeight(font))));
        }
    }
}
