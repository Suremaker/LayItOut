using System.Drawing;
using LayItOut.Attributes;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Attributes
{
    public class SizeTests
    {
        [Fact]
        public void ApplyIfSet_should_apply_proper_value()
        {
            var size = new Size(10, 20);
            size.ApplyIfSet(SizeUnit.NotSet, SizeUnit.Unlimited).ShouldBe(new Size(10, 20));
            size.ApplyIfSet(50, 3).ShouldBe(new Size(50, 3));
        }

        [Theory]
        [InlineData(10, 20, 5, 30, 5, 20)]
        [InlineData(5, 30, 10, 20, 5, 20)]
        public void Intersect_should_return_min_value(int w1, int h1, int w2, int h2, int expectedW, int expectedH)
        {
            new Size(w1, h1).Intersect(new Size(w2, h2)).ShouldBe(new Size(expectedW, expectedH));
        }

        [Theory]
        [InlineData(10, 20, 5, 30, 10, 30)]
        [InlineData(5, 30, 10, 20, 10, 30)]
        public void Union_should_return_max_value(int w1, int h1, int w2, int h2, int expectedW, int expectedH)
        {
            new Size(w1, h1).Union(new Size(w2, h2)).ShouldBe(new Size(expectedW, expectedH));
        }

        [Theory]
        [InlineData(2, 3, 2, 3)]
        [InlineData(2.1f, 3.05f, 3, 4)]
        public void Ceiling_should_convert_to_Size(float w, float h, int expectedW, int expectedH)
        {
            new SizeF(w, h).Ceiling().ShouldBe(new Size(expectedW, expectedH));
        }
    }
}