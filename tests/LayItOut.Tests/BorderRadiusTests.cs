using System;
using Shouldly;
using Xunit;

namespace LayItOut.Tests
{
    public class BorderRadiusTests
    {
        [Fact]
        public void It_should_initialize_properly_with_1_parameter()
        {
            var distance = 15;
            var spacer = new BorderRadius(distance);
            spacer.TopLeft.ShouldBe(distance);
            spacer.TopRight.ShouldBe(distance);
            spacer.BottomLeft.ShouldBe(distance);
            spacer.BottomRight.ShouldBe(distance);
            spacer.ToString().ShouldBe("15 15 15 15");
        }

        [Fact]
        public void It_should_initialize_properly_with_2_parameters()
        {
            var top = 15;
            var bottom = 25;
            var spacer = new BorderRadius(top, bottom);
            spacer.TopLeft.ShouldBe(top);
            spacer.TopRight.ShouldBe(top);
            spacer.BottomLeft.ShouldBe(bottom);
            spacer.BottomRight.ShouldBe(bottom);
            spacer.ToString().ShouldBe("15 15 25 25");
        }

        [Fact]
        public void It_should_initialize_properly_with_4_parameters()
        {
            var topLeft = 5;
            var topRight = 10;
            var bottomRight = 15;
            var bottomLeft = 20;
            var spacer = new BorderRadius(topLeft, topRight, bottomRight, bottomLeft);
            spacer.TopLeft.ShouldBe(topLeft);
            spacer.TopRight.ShouldBe(topRight);
            spacer.BottomLeft.ShouldBe(bottomLeft);
            spacer.BottomRight.ShouldBe(bottomRight);
            spacer.ToString().ShouldBe("5 10 15 20");
        }

        [Fact]
        public void Parse_should_properly_parse_the_border_radius()
        {
            BorderRadius.Parse("").ShouldBe(BorderRadius.None);
            BorderRadius.Parse("10").ShouldBe(new BorderRadius(10));
            BorderRadius.Parse("15 20").ShouldBe(new BorderRadius(15, 20));
            BorderRadius.Parse("15 20 30 40").ShouldBe(new BorderRadius(15, 20, 30, 40));
            BorderRadius.Parse(" 15 20 30 40 ").ShouldBe(new BorderRadius(15, 20, 30, 40));
            Assert.Throws<ArgumentException>(() => BorderRadius.Parse("x")).Message.ShouldStartWith("Provided value is not a valid BorderRadius: x");
            Assert.Throws<ArgumentException>(() => BorderRadius.Parse("10 20 30")).Message.ShouldStartWith("Provided value is not a valid BorderRadius: 10 20 30");
        }
    }
}