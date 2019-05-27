using System;
using System.Drawing;
using Shouldly;
using Xunit;

namespace LayItOut.Tests
{
    public class BorderLineTests
    {
        [Fact]
        public void Parse_should_handle_size_and_color()
        {
            BorderLine.Parse("10 red").ShouldBe(new BorderLine(10, Color.Red));
            BorderLine.Parse("* #112233").ShouldBe(new BorderLine(SizeUnit.Unlimited, Color.FromArgb(0x11, 0x22, 0x33)));
        }

        [Fact]
        public void Parse_should_handle_empty_value()
        {
            BorderLine.Parse("").ShouldBe(new BorderLine(SizeUnit.NotSet, Color.Empty));
        }

        [Fact]
        public void Parse_should_throw_meaningful_exception()
        {
            Assert.Throws<ArgumentException>(() => BorderLine.Parse("abc")).Message.StartsWith("Provided value is not a valid BorderLine: abc");
            Assert.Throws<ArgumentException>(() => BorderLine.Parse("abc def")).Message.StartsWith("Provided value is not a valid BorderLine: abc def");
        }
    }
}
