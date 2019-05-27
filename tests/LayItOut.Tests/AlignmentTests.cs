using System;
using System.Drawing;
using Shouldly;
using Xunit;

namespace LayItOut.Tests
{
    public class AlignmentTests
    {
        [Fact]
        public void Parse_should_properly_interpret_values()
        {
            Alignment.Parse("center").ShouldBe(new Alignment(VerticalAlignment.Center, HorizontalAlignment.Center));
            Alignment.Parse("ToP  lEfT").ShouldBe(new Alignment(VerticalAlignment.Top, HorizontalAlignment.Left));
            Alignment.Parse(" BOTTOM right ").ShouldBe(new Alignment(VerticalAlignment.Bottom, HorizontalAlignment.Right));
            Alignment.Parse("center CENTER").ShouldBe(new Alignment(VerticalAlignment.Center, HorizontalAlignment.Center));
            Assert.Throws<ArgumentException>(() => Alignment.Parse("foo")).Message.ShouldStartWith("Provided value is not a valid Alignment: foo");
        }

        [Fact]
        public void ToString_should_return_alignment()
        {
            new Alignment(VerticalAlignment.Bottom, HorizontalAlignment.Right).ToString().ShouldBe("Bottom Right");
            new Alignment(VerticalAlignment.Center, HorizontalAlignment.Center).ToString().ShouldBe("Center");
        }

        [Fact]
        public void Predefined_fields_should_have_proper_values()
        {
            Alignment.Center.ShouldBe(new Alignment(VerticalAlignment.Center, HorizontalAlignment.Center));
            Alignment.TopLeft.ShouldBe(new Alignment(VerticalAlignment.Top, HorizontalAlignment.Left));
        }

        [Theory]
        [InlineData(HorizontalAlignment.Center, 45, 22)]
        [InlineData(HorizontalAlignment.Left, 45, 0)]
        [InlineData(HorizontalAlignment.Right, 45, 45)]
        [InlineData(HorizontalAlignment.Right, -1, 0)]
        [InlineData(HorizontalAlignment.Left, -1, 0)]
        [InlineData(HorizontalAlignment.Center, -1, 0)]
        public void GetShift_should_calculate_horizontal_shift(HorizontalAlignment alignment, int remaining, int result)
        {
            alignment.GetShift(remaining).ShouldBe(new Size(result,0));
        }

        [Theory]
        [InlineData(VerticalAlignment.Center, 45, 22)]
        [InlineData(VerticalAlignment.Top, 45, 0)]
        [InlineData(VerticalAlignment.Bottom, 45, 45)]
        [InlineData(VerticalAlignment.Bottom, -1, 0)]
        [InlineData(VerticalAlignment.Top, -1, 0)]
        [InlineData(VerticalAlignment.Center, -1, 0)]
        public void GetShift_should_calculate_vertical_shift(VerticalAlignment alignment, int remaining, int result)
        {
            alignment.GetShift(remaining).ShouldBe(new Size(0,result));
        }

        [Theory]
        [InlineData(VerticalAlignment.Center, HorizontalAlignment.Center, 31, 41, 15, 20)]
        [InlineData(VerticalAlignment.Bottom, HorizontalAlignment.Left, 31, 41, 0, 41)]
        [InlineData(VerticalAlignment.Top, HorizontalAlignment.Right, 31, 41, 31, 0)]
        public void GetShift_should_calculate_alignment_shift(VerticalAlignment vertical, HorizontalAlignment horizontal, int width, int height, int expectedWidth, int expectedHeight)
        {
            new Alignment(vertical, horizontal).GetShift(new Size(width, height)).ShouldBe(new Size(expectedWidth, expectedHeight));
        }
    }
}
