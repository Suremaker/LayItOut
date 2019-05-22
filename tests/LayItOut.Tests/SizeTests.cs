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
    }
}