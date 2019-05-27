using System;
using System.Linq;
using Shouldly;
using Xunit;
// ReSharper disable EqualExpressionComparison
#pragma warning disable CS1718

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

        [Fact]
        public void Parse_should_properly_interpret_values()
        {
            SizeUnit.Parse("*").ShouldBe(SizeUnit.Unlimited);
            SizeUnit.Parse("").ShouldBe(SizeUnit.NotSet);
            SizeUnit.Parse(null).ShouldBe(SizeUnit.NotSet);
            SizeUnit.Parse(" ").ShouldBe(SizeUnit.NotSet);
            SizeUnit.Parse("15").ShouldBe(SizeUnit.Absolute(15));

            Assert.Throws<ArgumentException>(() => SizeUnit.Parse("x")).Message.ShouldStartWith("Provided value is not a valid SizeUnit: x");
        }

        [Fact]
        public void SizeUnit_should_have_equality_operators()
        {
            Assert.True(SizeUnit.Unlimited == SizeUnit.Unlimited);
            Assert.True(SizeUnit.NotSet == SizeUnit.NotSet);
            Assert.True(SizeUnit.Absolute(10) == SizeUnit.Absolute(10));

            Assert.False(SizeUnit.Unlimited == SizeUnit.NotSet);
            Assert.False(SizeUnit.NotSet == SizeUnit.Absolute(10));
            Assert.False(SizeUnit.Absolute(10) == SizeUnit.Unlimited);
            Assert.False(SizeUnit.Absolute(10) == SizeUnit.Absolute(11));

            Assert.False(SizeUnit.Unlimited != SizeUnit.Unlimited);
            Assert.False(SizeUnit.NotSet != SizeUnit.NotSet);
            Assert.False(SizeUnit.Absolute(10) != SizeUnit.Absolute(10));

            Assert.True(SizeUnit.Unlimited != SizeUnit.NotSet);
            Assert.True(SizeUnit.NotSet != SizeUnit.Absolute(10));
            Assert.True(SizeUnit.Absolute(10) != SizeUnit.Unlimited);
            Assert.True(SizeUnit.Absolute(10) != SizeUnit.Absolute(11));
        }

        [Fact]
        public void Distribute_returns_proper_values()
        {
            SizeUnit.Distribute(100, 7).ShouldBe(new[] { 14, 14, 14, 15, 14, 14, 15 });
            SizeUnit.Distribute(100, 999999).Sum().ShouldBe(100);
        }
    }
}
