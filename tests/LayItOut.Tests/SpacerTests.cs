using System;
using Shouldly;
using Xunit;

namespace LayItOut.Tests
{
    public class SpacerTests
    {
        [Fact]
        public void Spacer_should_initialize_properly_with_1_parameter()
        {
            var distance = SizeUnit.Absolute(15);
            var spacer = new Spacer(distance);
            spacer.Top.ShouldBe(distance);
            spacer.Bottom.ShouldBe(distance);
            spacer.Left.ShouldBe(distance);
            spacer.Right.ShouldBe(distance);
            spacer.ToString().ShouldBe("15 15 15 15");
        }

        [Fact]
        public void Spacer_should_initialize_properly_with_2_parameters()
        {
            var vertical = SizeUnit.Absolute(15);
            var horizontal = SizeUnit.Absolute(25);
            var spacer = new Spacer(vertical, horizontal);
            spacer.Top.ShouldBe(vertical);
            spacer.Bottom.ShouldBe(vertical);
            spacer.Left.ShouldBe(horizontal);
            spacer.Right.ShouldBe(horizontal);
            spacer.ToString().ShouldBe("15 25 15 25");
        }

        [Fact]
        public void Spacer_should_initialize_properly_with_4_parameters()
        {
            var top = SizeUnit.Absolute(15);
            var left = SizeUnit.Absolute(25);
            var bottom = SizeUnit.Absolute(35);
            var right = SizeUnit.Absolute(45);
            var spacer = new Spacer(top, left, bottom, right);
            spacer.Top.ShouldBe(top);
            spacer.Bottom.ShouldBe(bottom);
            spacer.Left.ShouldBe(left);
            spacer.Right.ShouldBe(right);
            spacer.ToString().ShouldBe("15 25 35 45");
        }

        [Fact]
        public void Parse_should_properly_parse_the_spacer()
        {
            Spacer.Parse("10").ShouldBe(new Spacer(10));
            Spacer.Parse("15 20").ShouldBe(new Spacer(15, 20));
            Spacer.Parse("15 20 30 40").ShouldBe(new Spacer(15, 20, 30, 40));
            Spacer.Parse(" 15 20 30 40 ").ShouldBe(new Spacer(15, 20, 30, 40));
            Assert.Throws<ArgumentException>(() => Spacer.Parse("x")).Message.ShouldStartWith("Provided value is not a valid Spacer: x");
            Assert.Throws<ArgumentException>(() => Spacer.Parse("10 20 30")).Message.ShouldStartWith("Provided value is not a valid Spacer: 10 20 30");
        }
    }
}
