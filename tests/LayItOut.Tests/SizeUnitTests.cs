using Shouldly;
using Xunit;

namespace LayItOut.Tests
{
    public class SizeUnitTests
    {
        [Fact]
        public void It_should_support_NotSet_value()
        {
            var x = new SizeUnit();
            x.ShouldBe(SizeUnit.NotSet);
            x.Value.ShouldBe(0);
            x.IsAbsolute.ShouldBeFalse();
            x.IsUnlimited.ShouldBeFalse();
            x.IsNotSet.ShouldBeTrue();
            x.ToString().ShouldBe("not set");
        }

        [Fact]
        public void It_should_support_Unlimited_value()
        {
            var x = SizeUnit.Unlimited;
            x.ShouldBe(SizeUnit.Unlimited);
            x.Value.ShouldBe(0);
            x.IsAbsolute.ShouldBeFalse();
            x.IsUnlimited.ShouldBeTrue();
            x.IsNotSet.ShouldBeFalse();
            x.ToString().ShouldBe("*");
        }

        [Fact]
        public void It_should_support_Absolute_value()
        {
            var x = SizeUnit.Absolute(10);
            x.Value.ShouldBe(10);
            x.IsAbsolute.ShouldBeTrue();
            x.IsUnlimited.ShouldBeFalse();
            x.IsNotSet.ShouldBeFalse();
            x.ToString().ShouldBe("10");

            SizeUnit y = 10;
            y.ShouldBe(x);
            SizeUnit.Zero.Value.ShouldBe(0);
        }

        [Fact]
        public void ApplyIfSet_should_apply_proper_value()
        {
            var input = 10;
            SizeUnit.Unlimited.ApplyIfSet(input).ShouldBe(10);
            SizeUnit.NotSet.ApplyIfSet(input).ShouldBe(10);
            SizeUnit.Absolute(30).ApplyIfSet(input).ShouldBe(30);
        }
    }
}
