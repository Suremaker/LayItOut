using System.Drawing;
using Shouldly;
using Xunit;

namespace LayItOut.Tests
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
    }
}