using System.Drawing;
using LayItOut.Attributes;
using LayItOut.Components;
using LayItOut.Tests.TestHelpers;
using Moq;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Components
{
    public class ViewportTests
    {
        [Fact]
        public void GetChildren_should_return_inner_component()
        {
            var component = Mock.Of<IComponent>();

            var viewport = new Viewport { Inner = component };
            viewport.GetChildren().ShouldBe(new[] { component });

            viewport.Inner = null;
            viewport.GetChildren().ShouldBeEmpty();
        }

        [Fact]
        public void Measure_should_include_ClipMargin()
        {
            var viewport = new Viewport
            {
                Inner = new FixedMeasureComponent(100, 200),
                ClipMargin = new Spacer(10, 20, 30, 40)
            };
            viewport.Measure(new Size(int.MaxValue, int.MaxValue), TestRendererContext.Instance);
            viewport.DesiredSize.ShouldBe(new Size(40, 160));
        }

        [Theory]
        [InlineData("top left", "0", "1:2|100x200", "1:2|30x30")]
        [InlineData("top left", "10 20 0 0", "-19:-8|100x200", "1:2|30x30")]
        [InlineData("top left", "0 0 185 90", "1:2|100x200", "1:2|10x15")]
        [InlineData("bottom right", "0", "-69:-168|100x200", "1:2|30x30")]
        [InlineData("bottom right", "10", "-59:-158|100x200", "1:2|30x30")]
        [InlineData("center", "0", "-34:-83|100x200", "1:2|30x30")]
        [InlineData("center", "91 40", "-34:-83|100x200", "1:2|20x18")]
        public void Arrange_should_arrange_inner_component(string contentAlignment, string clipMargin, string expectedInnerLayout, string expectedViewRegion)
        {
            var viewport = new Viewport
            {
                Width = 30,
                Height = 30,
                ClipMargin = Spacer.Parse(clipMargin),
                Inner = new FixedMeasureComponent(100, 200),
                ContentAlignment = Alignment.Parse(contentAlignment)
            };
            viewport.Measure(new Size(int.MaxValue, int.MaxValue), TestRendererContext.Instance);

            viewport.Arrange(new Rectangle(1, 2, 30, 30));
            viewport.Inner.Layout.ShouldBe(RectParser.ToRect(expectedInnerLayout));
            viewport.ActualViewRegion.ShouldBe(RectParser.ToRect(expectedViewRegion));
        }
    }
}