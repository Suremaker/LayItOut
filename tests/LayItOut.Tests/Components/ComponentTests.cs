using System;
using System.Drawing;
using LayItOut.Components;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Components
{
    public class ComponentTests
    {
        class TestableComponent : Component
        {
            public Action<Rectangle> OnArrangeCallback = _ => { };
            public Func<Size, Size> OnMeasureCallback = size => size;

            protected override Size OnMeasure(Size size) => OnMeasureCallback(size);
            protected override void OnArrange() => OnArrangeCallback(Layout);
        }

        [Fact]
        public void GetChildren_returns_empty_collection()
        {
            new Component().GetChildren().ShouldBeEmpty();
        }

        [Fact]
        public void Measure_should_update_DesiredSize_allowing_it_being_higher_than_input()
        {
            var component = new TestableComponent
            {
                OnMeasureCallback = s => new Size(100, 200)
            };
            component.Measure(new Size(5, 10));
            component.DesiredSize.ShouldBe(new Size(100, 200));
        }

        [Fact]
        public void Component_Measure_should_honor_Width_and_Height_but_otherwise_set_0()
        {
            var size = new Size(2, 3);
            var component = new Component { Width = 10, Height = SizeUnit.Unlimited };
            component.Measure(size);
            component.DesiredSize.ShouldBe(new Size(10, 0));

            component.Width = SizeUnit.Unlimited;
            component.Height = 10;
            component.Measure(size);
            component.DesiredSize.ShouldBe(new Size(0, 10));
        }

        [Theory]
        [InlineData("", "", 15, 20, 20, 30)]
        [InlineData("*", "*", 15, 20, 20, 30)]
        [InlineData("", "5", 15, 5, 20, 5)]
        [InlineData("40", "", 40, 20, 40, 30)]
        public void Measure_should_use_Width_and_Height_if_provided(string width, string height, int expectedWidth, int expectedHeight, int desiredWidth, int desiredHeight)
        {
            var inputSize = new Size(15, 20);
            var fixedMeasureSize = new Size(20, 30);
            var receivedInputSize = Size.Empty;

            var component = new TestableComponent
            {
                Width = SizeUnit.Parse(width),
                Height = SizeUnit.Parse(height),
                OnMeasureCallback = x =>
                {
                    receivedInputSize = x;
                    return fixedMeasureSize;
                }
            };
            component.Measure(inputSize);

            component.DesiredSize.ShouldBe(new Size(desiredWidth, desiredHeight));
            receivedInputSize.ShouldBe(new Size(expectedWidth, expectedHeight));
        }

        [Fact]
        public void Arrange_should_update_layout()
        {
            var expected = new Rectangle(5, 10, 15, 20);
            var received = Rectangle.Empty;
            var component = new TestableComponent { OnArrangeCallback = actual => received = actual };

            component.Measure(expected.Size);
            component.Arrange(expected);

            component.Layout.ShouldBe(expected, "Layout was not updated");
            received.ShouldBe(expected, "OnArrange was not called");
        }

        [Theory]
        [InlineData("center", 20 + 45, 30 + 40, 10, 20)]
        [InlineData("bottom right", 20 + 90, 30 + 80, 10, 20)]
        [InlineData("top left", 20, 30, 10, 20)]
        public void Arrange_should_align_component_if_DesiredSize_is_smaller_than_boundingBox(string alignment, int expectedX, int expectedY, int expectedWidth, int expectedHeight)
        {
            var area = new Rectangle(20, 30, 100, 100);
            var component = new TestableComponent
            {
                OnMeasureCallback = _ => new Size(10, 20),
                Alignment = Alignment.Parse(alignment)
            };
            component.Measure(area.Size);
            component.Arrange(area);
            component.Layout.ShouldBe(new Rectangle(expectedX, expectedY, expectedWidth, expectedHeight));
        }

        [Theory]
        [InlineData("*", "*", 20, 30, 100, 100)]
        [InlineData("", "*", 20, 30, 10, 100)]
        [InlineData("*", "", 20, 30, 100, 20)]
        public void Arrange_should_expand_component_if_Width_or_Height_is_unlimited(string width, string height, int expectedX, int expectedY, int expectedWidth, int expectedHeight)
        {
            var area = new Rectangle(20, 30, 100, 100);
            var component = new TestableComponent
            {
                OnMeasureCallback = _ => new Size(10, 20),
                Width = SizeUnit.Parse(width),
                Height = SizeUnit.Parse(height)
            };
            component.Measure(area.Size);
            component.Arrange(area);
            component.Layout.ShouldBe(new Rectangle(expectedX, expectedY, expectedWidth, expectedHeight));
        }

        [Fact]
        public void Arrange_should_not_get_over_provided_dimensions()
        {
            var area = new Rectangle(20, 30, 45, 35);
            var component = new Component { Width = 100, Height = 100 };
            component.Measure(area.Size);
            component.Arrange(area);
            component.Layout.ShouldBe(area);
            component.DesiredSize.ShouldBe(new Size(100, 100));
        }
    }
}
