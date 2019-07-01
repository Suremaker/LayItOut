using System.Drawing;
using System.Linq;
using LayItOut.Components;
using LayItOut.Tests.TestHelpers;
using Moq;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Components
{
    public class VBoxTests
    {
        [Fact]
        public void Measure_should_sum_height_and_take_biggest_width()
        {
            var box = new VBox();
            box.AddComponent(new FixedMeasureComponent(22, 22));
            box.AddComponent(new FixedMeasureComponent(11, 11));
            box.AddComponent(new FixedMeasureComponent(33, 33));
            box.Measure(new Size(100, 100), TestRendererContext.Instance);

            box.DesiredSize.ShouldBe(new Size(33, 66));
        }

        [Fact]
        public void Measure_should_pass_size_to_the_component_with_height_decreased_by_the_height_of_previous_ones_but_not_smaller_than_0()
        {
            var box = new VBox();
            var c1 = new Mock<IComponent>().WithDesiredSize(new Size(10, 15));
            var c2 = new Mock<IComponent>().WithDesiredSize(new Size(20, 20));
            var c3 = new Mock<IComponent>().WithDesiredSize(new Size(20, 20));

            box.AddComponent(c1.Object);
            box.AddComponent(c2.Object);
            box.AddComponent(c3.Object);
            box.Measure(new Size(100, 30), TestRendererContext.Instance);

            c1.Verify(x => x.Measure(new Size(100, 30), TestRendererContext.Instance));
            c2.Verify(x => x.Measure(new Size(100, 15), TestRendererContext.Instance));
            c3.Verify(x => x.Measure(new Size(100, 0), TestRendererContext.Instance));
        }

        [Fact]
        public void Arrange_should_layout_children_vertically_passing_max_width_but_limiting_area_to_not_expand_over_the_bounding_box()
        {
            var area = new Rectangle(5, 5, 100, 30);

            var box = new VBox();
            var c1 = new Component { Width = 90, Height = 15 };
            var c2 = new Component { Width = 110, Height = 20 };
            var c3 = new Component { Width = 100, Height = 20 };
            box.AddComponent(c1);
            box.AddComponent(c2);
            box.AddComponent(c3);

            box.Measure(area.Size, TestRendererContext.Instance);
            box.Arrange(area);

            c1.Layout.ShouldBe(new Rectangle(5, 5, 90, 15));
            c2.Layout.ShouldBe(new Rectangle(5, 5 + 15, 100, 15));
            c3.Layout.ShouldBe(new Rectangle(5, 5 + 15 + 15, 100, 0));
        }

        [Theory]
        [InlineData(VerticalAlignment.Center)]
        [InlineData(VerticalAlignment.Top)]
        [InlineData(VerticalAlignment.Bottom)]
        public void Arrange_should_use_ContentAlignment_to_layout_children_if_they_are_not_taking_whole_space(VerticalAlignment alignment)
        {
            var area = new Rectangle(5, 5, 100, 100);
            var box = new VBox { Height = 100, ContentAlignment = alignment };
            var c1 = new Component { Width = 100, Height = 20 };
            var c2 = new Component { Width = 100, Height = 20 };
            var c3 = new Component { Width = 100, Height = 20 };
            box.AddComponent(c1);
            box.AddComponent(c2);
            box.AddComponent(c3);

            box.Measure(area.Size, TestRendererContext.Instance);
            box.Arrange(area);
            var totalChildrenHeight = box.GetChildren().Sum(x => x.DesiredSize.Height);
            var remainingHeight = area.Height - totalChildrenHeight;

            var shiftY = alignment == VerticalAlignment.Center
                ? remainingHeight / 2
                : (alignment == VerticalAlignment.Bottom ? remainingHeight : 0);
            var shift = new Size(0, shiftY);

            c1.Layout.ShouldBe(new Rectangle(area.Location + shift, c1.DesiredSize));
            c2.Layout.ShouldBe(new Rectangle(new Point(area.Left, c1.Layout.Bottom), c2.DesiredSize));
            c3.Layout.ShouldBe(new Rectangle(new Point(area.Left, c2.Layout.Bottom), c3.DesiredSize));
        }

        [Theory]
        [InlineData(VerticalAlignment.Center)]
        [InlineData(VerticalAlignment.Top)]
        [InlineData(VerticalAlignment.Bottom)]
        public void Arrange_should_ignore_ContentAlignment_if_children_are_taking_the_whole_space(VerticalAlignment alignment)
        {
            var area = new Rectangle(5, 5, 100, 100);
            var box = new VBox { Height = 100, ContentAlignment = alignment };
            var c1 = new Component { Height = 110, Width = 120 };
            box.AddComponent(c1);

            box.Measure(area.Size, TestRendererContext.Instance);
            box.Arrange(area);
            c1.Layout.ShouldBe(area);
        }

        
        [Fact]
        public void Arrange_should_layout_items_honoring_their_desired_size()
        {
            var area = new Rectangle(5, 5, 100, 100);
            var box = new VBox { Height = 100};
            var c1 = new Component { Height = 40, Width = 100 };
            var c2 = new TestableComponent { Height = SizeUnit.Unlimited,  Width= 100,OnMeasureCallback = _=>new Size(100,50)};
            var c3 = new Component { Height = 40, Width = 100 };
            box.AddComponent(c1);
            box.AddComponent(c2);
            box.AddComponent(c3);

            box.Measure(area.Size, TestRendererContext.Instance);
            box.Arrange(area);
            c1.Layout.ShouldBe(new Rectangle(5,5,100,40));
            c2.Layout.ShouldBe(new Rectangle(5,45,100,50));
            c3.Layout.ShouldBe(new Rectangle(5,95,100,10));
        }

        [Theory]
        [InlineData(VerticalAlignment.Center)]
        [InlineData(VerticalAlignment.Top)]
        [InlineData(VerticalAlignment.Bottom)]
        public void Arrange_should_distribute_remaining_width_between_components_with_unlimited_width_ignoring_ContentAlignment(VerticalAlignment alignment)
        {
            var area = new Rectangle(5, 5, 100, 100);

            var box = new VBox
            {
                ContentAlignment = alignment,
                Height = 100
            };
            var c1 = new FixedMeasureComponent(100, 20) { Height = SizeUnit.Unlimited };
            var c2 = new Component { Width = 100, Height = 25 };
            var c3 = new FixedMeasureComponent(100, 30) { Height = SizeUnit.Unlimited };

            box.AddComponent(c1);
            box.AddComponent(c2);
            box.AddComponent(c3);

            box.Measure(area.Size, TestRendererContext.Instance);
            box.Arrange(area);

            c1.Layout.ShouldBe(new Rectangle(5, 5, 100, 20 + 12));
            c2.Layout.ShouldBe(new Rectangle(new Point(area.Left, c1.Layout.Bottom), new Size(100, 25)));
            c3.Layout.ShouldBe(new Rectangle(new Point(area.Left, c2.Layout.Bottom), new Size(100, 30 + 13)));
        }
    }
}