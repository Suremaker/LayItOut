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
            Border.Parse("10 red").ShouldBe(new Border(10, Color.Red));
            Border.Parse("1 #112233").ShouldBe(new Border(1, Color.FromArgb(0x11, 0x22, 0x33)));
        }

        [Fact]
        public void Parse_should_handle_empty_value()
        {
            Border.Parse("").ShouldBe(new Border(0, Color.Empty));
        }

        [Fact]
        public void Parse_should_throw_meaningful_exception()
        {
            Assert.Throws<ArgumentException>(() => Border.Parse("abc")).Message.StartsWith("Provided value is not a valid Border: abc");
            Assert.Throws<ArgumentException>(() => Border.Parse("abc def")).Message.StartsWith("Provided value is not a valid Border: abc def");
        }
    }
}
