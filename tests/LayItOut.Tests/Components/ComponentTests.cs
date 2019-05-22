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
            private readonly Action<Rectangle> _onArrangeCallback;

            public TestableComponent(Action<Rectangle> onArrangeCallback = null)
            {
                _onArrangeCallback = onArrangeCallback ?? DummyCallback;
            }
            protected override Size OnMeasure(Size size) => size + size;
            protected override void OnArrange() => _onArrangeCallback(Layout);
            private void DummyCallback(Rectangle obj) { }
        }

        [Fact]
        public void GetChildren_returns_empty_collection()
        {
            new Component().GetChildren().ShouldBeEmpty();
        }

        [Fact]
        public void Measure_should_update_DesiredSize()
        {
            var component = new TestableComponent();
            component.Measure(new Size(5, 10));
            component.DesiredSize.ShouldBe(new Size(10, 20));
        }

        [Fact]
        public void Arrange_should_update_layout()
        {
            var expected = new Rectangle(5, 10, 15, 20);
            var received = Rectangle.Empty;
            var component = new TestableComponent(actual => received = actual);

            component.Arrange(expected);
            component.Layout.ShouldBe(expected, "Layout was not updated");
            received.ShouldBe(expected, "OnArrange was not called");
        }
    }
}
