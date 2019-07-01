using System.Drawing;
using System.Linq;
using LayItOut.Components;
using LayItOut.Tests.TestHelpers;
using Moq;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Components
{
    public class StackTests
    {
        [Fact]
        public void Measure_should_stretch_to_fit_the_biggest_component()
        {
            var stack = new Stack();
            stack.AddComponent(new FixedMeasureComponent(10, 20));
            stack.AddComponent(new FixedMeasureComponent(30, 5));
            stack.AddComponent(new FixedMeasureComponent(5, 10));

            stack.Measure(new Size(100, 100), TestRendererContext.Instance);

            stack.DesiredSize.ShouldBe(new Size(30, 20));
        }

        [Fact]
        public void Measure_should_pass_given_size_to_children()
        {
            var size = new Size(10, 20);

            var child = new Mock<IComponent>();
            var stack = new Stack();
            stack.AddComponent(child.Object);

            stack.Measure(size, TestRendererContext.Instance);

            child.Verify(x => x.Measure(size, TestRendererContext.Instance));
        }

        [Fact]
        public void Arrange_should_stack_children_on_each_other_using_calculated_layout()
        {
            var area = new Rectangle(10, 20, 30, 40);
            var stack = new Stack { Width = 20, Height = 30, Alignment = Alignment.Center };
            var children = Enumerable.Range(0, 5).Select(_ => new Mock<IComponent>()).ToArray();
            foreach (var child in children)
                stack.AddComponent(child.Object);

            stack.Measure(area.Size, TestRendererContext.Instance);
            stack.Arrange(area);

            stack.Layout.ShouldBe(new Rectangle(15, 25, 20, 30));
            foreach (var child in children)
                child.Verify(x => x.Arrange(stack.Layout));
        }
    }
}
