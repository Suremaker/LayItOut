using System.Drawing;
using System.Linq;
using LayItOut.Components;
using LayItOut.Tests.TestHelpers;
using Moq;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Components
{
    public class HBoxTests
    {
        [Fact]
        public void Measure_should_sum_width_and_take_biggest_height()
        {
            var box = new HBox();
            box.AddComponent(new FixedMeasureComponent(22, 22));
            box.AddComponent(new FixedMeasureComponent(11, 11));
            box.AddComponent(new FixedMeasureComponent(33, 33));
            box.Measure(new Size(100, 100), TestRendererContext.Instance);

            box.DesiredSize.ShouldBe(new Size(66, 33));
        }

        [Fact]
        public void Measure_should_pass_size_to_the_component_with_width_decreased_by_the_width_of_previous_ones_but_not_smaller_than_0()
        {
            var box = new HBox();
            var c1 = new Mock<IComponent>().WithDesiredSize(new Size(15, 10));
            var c2 = new Mock<IComponent>().WithDesiredSize(new Size(20, 20));
            var c3 = new Mock<IComponent>().WithDesiredSize(new Size(20, 20));

            box.AddComponent(c1.Object);
            box.AddComponent(c2.Object);
            box.AddComponent(c3.Object);
            box.Measure(new Size(30, 100), TestRendererContext.Instance);

            c1.Verify(x => x.Measure(new Size(30, 100), TestRendererContext.Instance));
            c2.Verify(x => x.Measure(new Size(15, 100), TestRendererContext.Instance));
            c3.Verify(x => x.Measure(new Size(0, 100), TestRendererContext.Instance));
        }

        [Fact]
        public void Arrange_should_layout_children_horizontally_passing_max_height_but_limiting_area_to_not_expand_over_the_bounding_box()
        {
            var area = new Rectangle(5, 5, 30, 100);

            var box = new HBox();
            var c1 = new Component { Width = 15, Height = 90 };
            var c2 = new Component { Width = 20, Height = 110 };
            var c3 = new Component { Width = 20, Height = 100 };
            box.AddComponent(c1);
            box.AddComponent(c2);
            box.AddComponent(c3);

            box.Measure(area.Size, TestRendererContext.Instance);
            box.Arrange(area);

            c1.Layout.ShouldBe(new Rectangle(5, 5, 15, 90));
            c2.Layout.ShouldBe(new Rectangle(5 + 15, 5, 15, 100));
            c3.Layout.ShouldBe(new Rectangle(5 + 15 + 15, 5, 0, 100));
        }

        [Theory]
        [InlineData(HorizontalAlignment.Center)]
        [InlineData(HorizontalAlignment.Left)]
        [InlineData(HorizontalAlignment.Right)]
        public void Arrange_should_use_ContentAlignment_to_layout_children_if_they_are_not_taking_whole_space(HorizontalAlignment alignment)
        {
            var area = new Rectangle(5, 5, 100, 100);
            var box = new HBox { Width = 100, ContentAlignment = alignment };
            var c1 = new Component { Width = 20, Height = 100 };
            var c2 = new Component { Width = 20, Height = 100 };
            var c3 = new Component { Width = 20, Height = 100 };
            box.AddComponent(c1);
            box.AddComponent(c2);
            box.AddComponent(c3);

            box.Measure(area.Size, TestRendererContext.Instance);
            box.Arrange(area);
            var totalChildrenWidth = box.GetChildren().Sum(x => x.DesiredSize.Width);
            var remainingWidth = area.Width - totalChildrenWidth;

            var shiftX = alignment == HorizontalAlignment.Center
                ? remainingWidth / 2
                : (alignment == HorizontalAlignment.Right ? remainingWidth : 0);
            var shift = new Size(shiftX, 0);

            c1.Layout.ShouldBe(new Rectangle(area.Location + shift, c1.DesiredSize));
            c2.Layout.ShouldBe(new Rectangle(new Point(c1.Layout.Right, area.Top), c2.DesiredSize));
            c3.Layout.ShouldBe(new Rectangle(new Point(c2.Layout.Right, area.Top), c3.DesiredSize));
        }

        [Theory]
        [InlineData(HorizontalAlignment.Center)]
        [InlineData(HorizontalAlignment.Left)]
        [InlineData(HorizontalAlignment.Right)]
        public void Arrange_should_ignore_ContentAlignment_if_children_are_taking_the_whole_space(HorizontalAlignment alignment)
        {
            var area = new Rectangle(5, 5, 100, 100);
            var box = new HBox { Width = 100, ContentAlignment = alignment };
            var c1 = new Component { Width = 120, Height = 110 };
            box.AddComponent(c1);

            box.Measure(area.Size, TestRendererContext.Instance);
            box.Arrange(area);
            c1.Layout.ShouldBe(area);
        }

        [Fact]
        public void Arrange_should_layout_items_honoring_their_desired_size()
        {
            var area = new Rectangle(5, 5, 100, 100);
            var box = new HBox { Width = 100};
            var c1 = new Component { Width = 40, Height = 100 };
            var c2 = new TestableComponent { Width = SizeUnit.Unlimited, Height = 100,OnMeasureCallback = _=>new Size(50,100)};
            var c3 = new Component { Width = 40, Height = 100 };
            box.AddComponent(c1);
            box.AddComponent(c2);
            box.AddComponent(c3);

            box.Measure(area.Size, TestRendererContext.Instance);
            box.Arrange(area);
            c1.Layout.ShouldBe(new Rectangle(5,5,40,100));
            c2.Layout.ShouldBe(new Rectangle(45,5,50,100));
            c3.Layout.ShouldBe(new Rectangle(95,5,10,100));
        }

        [Theory]
        [InlineData(HorizontalAlignment.Center)]
        [InlineData(HorizontalAlignment.Left)]
        [InlineData(HorizontalAlignment.Right)]
        public void Arrange_should_distribute_remaining_width_between_components_with_unlimited_width_ignoring_ContentAlignment(HorizontalAlignment alignment)
        {
            var area = new Rectangle(5, 5, 100, 100);

            var box = new HBox
            {
                ContentAlignment = alignment,
                Width = 100
            };
            var c1 = new FixedMeasureComponent(20, 100) { Width = SizeUnit.Unlimited };
            var c2 = new Component { Width = 25, Height = 100 };
            var c3 = new FixedMeasureComponent(30, 100) { Width = SizeUnit.Unlimited };

            box.AddComponent(c1);
            box.AddComponent(c2);
            box.AddComponent(c3);

            box.Measure(area.Size, TestRendererContext.Instance);
            box.Arrange(area);

            c1.Layout.ShouldBe(new Rectangle(5, 5, 20 + 12, 100));
            c2.Layout.ShouldBe(new Rectangle(new Point(c1.Layout.Right, area.Top), new Size(25, 100)));
            c3.Layout.ShouldBe(new Rectangle(new Point(c2.Layout.Right, area.Top), new Size(30 + 13, 100)));
        }
    }
}
