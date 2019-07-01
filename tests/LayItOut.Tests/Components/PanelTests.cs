using System.Drawing;
using LayItOut.Components;
using LayItOut.Tests.TestHelpers;
using Moq;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Components
{
    public class PanelTests
    {
        [Fact]
        public void GetChildren_should_return_inner_component()
        {
            var component = Mock.Of<IComponent>();

            var panel = new Panel { Inner = component };
            panel.GetChildren().ShouldBe(new[] { component });

            panel.Inner = null;
            panel.GetChildren().ShouldBeEmpty();
        }

        [Fact]
        public void Measure_should_include_Margin_Padding_Border_and_Inner_component()
        {
            var panel = new Panel
            {
                Inner = new FixedMeasureComponent(1, 2),
                Margin = new Spacer(10, 20, 30, 40),
                Padding = new Spacer(100, 200, 300, 400),
                Border = new Border(1000, Color.Black)
            };
            panel.Measure(new Size(int.MaxValue, int.MaxValue), TestRendererContext.Instance);
            panel.DesiredSize.ShouldBe(new Size(2661, 2442));
        }

        [Theory]
        [InlineData("10 -", "1 red", "* 5", 2*1+2*5+10,2*10+2*1+10)]
        [InlineData("10 5 * -", "", "- * 30 2", 10 + 5 + 2, 10 + 10 + 30)]
        public void Measure_should_only_include_absolute_values_of_Margin_and_Padding(string margin, string border, string padding, int expectedWidth, int expectedHeight)
        {
            var panel = new Panel
            {
                Inner = new FixedMeasureComponent(10, 10),
                Margin = Spacer.Parse(margin),
                Padding = Spacer.Parse(padding),
                Border = Border.Parse(border)
            };
            panel.Measure(new Size(int.MaxValue, int.MaxValue), TestRendererContext.Instance);
            panel.DesiredSize.ShouldBe(new Size(expectedWidth, expectedHeight));
        }

        [Fact]
        public void Measure_should_work_without_inner_component()
        {
            var panel = new Panel
            {
                Margin = new Spacer(10, 20, 30, 40),
                Padding = new Spacer(100, 200, 300, 400),
                Border = new Border(1000, Color.Black)
            };
            panel.Measure(new Size(int.MaxValue, int.MaxValue), TestRendererContext.Instance);
            panel.DesiredSize.ShouldBe(new Size(2660, 2440));
        }

        [Theory]
        [InlineData(200, 200, 100, 140)]
        [InlineData(60, 60, 0, 0)]
        public void Measure_should_pass_size_to_Inner_component_decreased_by_Margin_Padding_and_Border_but_not_less_than_0(int width, int height, int expectedWidth, int expectedHeight)
        {
            var captured = Size.Empty;
            var inner = new TestableComponent
            {
                OnMeasureCallback = x =>
                {
                    captured = x;
                    return x;
                }
            };

            var panel = new Panel
            {
                Inner = inner,
                Margin = new Spacer(10, 20, 10, 20),
                Padding = new Spacer(10, 20, 10, 20),
                Border = new Border(10,Color.Black)};

            panel.Measure(new Size(width, height), TestRendererContext.Instance);
            captured.ShouldBe(new Size(expectedWidth, expectedHeight));
        }

        [Fact]
        public void Arrange_should_layout_margin_then_border_then_padding_then_inner()
        {
            var area = new Rectangle(5, 5, 100, 100);
            var panel = new Panel
            {
                Margin = new Spacer(10),
                Border = new Border(1, Color.Black),
                Padding = new Spacer(5),
                Inner = new Component { Width = 20, Height = 10 }
            };

            panel.Measure(area.Size, TestRendererContext.Instance);
            panel.Arrange(area);

            panel.Layout.ShouldBe(new Rectangle(5, 5, 52, 42));
            panel.BorderLayout.ShouldBe(new Rectangle(new Point(panel.Layout.Left + 10, panel.Layout.Top + 10), new Size(32, 22)));
            panel.PaddingLayout.ShouldBe(new Rectangle(new Point(panel.BorderLayout.Left + 1, panel.BorderLayout.Top + 1), new Size(30, 20)));
            panel.Inner.Layout.ShouldBe(new Rectangle(new Point(panel.PaddingLayout.Left + 5, panel.PaddingLayout.Top + 5), new Size(20, 10)));
            panel.ActualBorder.ShouldBe(new Spacer(1));
        }

        [Fact]
        public void Arrange_should_trim_inner_component_if_area_is_too_small()
        {
            var area = new Rectangle(5, 5, 65, 70);
            var panel = new Panel
            {
                Margin = new Spacer(10),
                Border = new Border(10, Color.Black),
                Padding = new Spacer(10),
                Inner = new Component { Width = 20, Height = 20 }
            };

            panel.Measure(area.Size, TestRendererContext.Instance);
            panel.Arrange(area);

            panel.Layout.ShouldBe(new Rectangle(5, 5, 65, 70));
            panel.BorderLayout.ShouldBe(new Rectangle(new Point(panel.Layout.Left + 10, panel.Layout.Top + 10), new Size(45, 50)));
            panel.PaddingLayout.ShouldBe(new Rectangle(new Point(panel.BorderLayout.Left + 10, panel.BorderLayout.Top + 10), new Size(25, 30)));
            panel.Inner.Layout.ShouldBe(new Rectangle(new Point(panel.PaddingLayout.Left + 10, panel.PaddingLayout.Top + 10), new Size(5, 10)));
            panel.ActualBorder.ShouldBe(new Spacer(10));
        }

        [Fact]
        public void Arrange_should_trim_inner_component_and_padding_if_area_is_too_small()
        {
            var area = new Rectangle(5, 5, 45, 50);
            var panel = new Panel
            {
                Margin = new Spacer(10),
                Border = new Border(10, Color.Black),
                Padding = new Spacer(10),
                Inner = new Component { Width = 20, Height = 20 }
            };

            panel.Measure(area.Size, TestRendererContext.Instance);
            panel.Arrange(area);

            panel.Layout.ShouldBe(new Rectangle(5, 5, 45, 50));
            panel.BorderLayout.ShouldBe(new Rectangle(new Point(panel.Layout.Left + 10, panel.Layout.Top + 10), new Size(25, 30)));
            panel.PaddingLayout.ShouldBe(new Rectangle(new Point(panel.BorderLayout.Left + 10, panel.BorderLayout.Top + 10), new Size(5, 10)));
            panel.Inner.Layout.ShouldBe(new Rectangle(new Point(panel.PaddingLayout.Left + 5, panel.PaddingLayout.Top + 10), new Size(0, 0)));
            panel.ActualBorder.ShouldBe(new Spacer(10));
        }

        [Fact]
        public void Arrange_should_trim_inner_component_and_padding_and_border_if_area_is_too_small()
        {
            var area = new Rectangle(5, 5, 25, 30);
            var panel = new Panel
            {
                Margin = new Spacer(10),
                Border = new Border(10, Color.Black),
                Padding = new Spacer(10),
                Inner = new Component { Width = 20, Height = 20 }
            };

            panel.Measure(area.Size, TestRendererContext.Instance);
            panel.Arrange(area);

            panel.Layout.ShouldBe(new Rectangle(5, 5, 25, 30));
            panel.BorderLayout.ShouldBe(new Rectangle(new Point(panel.Layout.Left + 10, panel.Layout.Top + 10), new Size(5, 10)));
            panel.PaddingLayout.ShouldBe(new Rectangle(new Point(panel.BorderLayout.Left + 5, panel.BorderLayout.Top + 10), new Size(0, 0)));
            panel.Inner.Layout.ShouldBe(new Rectangle(new Point(panel.PaddingLayout.Left, panel.PaddingLayout.Top), new Size(0, 0)));
            panel.ActualBorder.ShouldBe(new Spacer(10, 5, 0, 0));
        }

        [Fact]
        public void Arrange_should_trim_inner_component_and_padding_and_border_and_margin_if_area_is_too_small()
        {
            var area = new Rectangle(5, 5, 5, 10);
            var panel = new Panel
            {
                Margin = new Spacer(10),
                Border = new Border(10, Color.Black),
                Padding = new Spacer(10),
                Inner = new Component { Width = 20, Height = 20 }
            };

            panel.Measure(area.Size, TestRendererContext.Instance);
            panel.Arrange(area);

            panel.Layout.ShouldBe(new Rectangle(5, 5, 5, 10));
            panel.BorderLayout.ShouldBe(new Rectangle(new Point(panel.Layout.Left + 5, panel.Layout.Top + 10), new Size(0, 0)));
            panel.PaddingLayout.ShouldBe(new Rectangle(new Point(panel.BorderLayout.Left, panel.BorderLayout.Top), new Size(0, 0)));
            panel.Inner.Layout.ShouldBe(new Rectangle(new Point(panel.PaddingLayout.Left, panel.PaddingLayout.Top), new Size(0, 0)));
            panel.ActualBorder.ShouldBe(new Spacer(0));
        }

        [Fact]
        public void Arrange_should_honor_unlimited_sizes()
        {
            var panel = new Panel
            {
                Inner = new Component { Width = SizeUnit.Unlimited, Height = SizeUnit.Unlimited },
                Margin = Spacer.Parse("* 10 * 10"),
                Border = Border.Parse("5 red"),
                Padding = Spacer.Parse("10 0 0 *"),
                Width = SizeUnit.Unlimited,
                Height = SizeUnit.Unlimited
            };

            var size = new Size(100, 100);
            panel.Measure(size, TestRendererContext.Instance);
            panel.Arrange(new Rectangle(Point.Empty, size));

            //hor 2
            //ver 3
            //tot w 10+10+10 = 30 => 100-30 = 70 / 2 = 35
            //tot h 10+10 = 30 => 100-30 = 80 / 3 = 26.6
            panel.BorderLayout.ShouldBe(new Rectangle(10, 26, 80, 47));
            panel.PaddingLayout.ShouldBe(new Rectangle(15, 31, 70, 37));
            panel.Inner.Layout.ShouldBe(new Rectangle(15, 41, 35, 27));
        }
    }
}
