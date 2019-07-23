using System;
using System.Drawing;
using LayItOut.Loaders;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Loaders
{
    public class ColorParserTests
    {
        [Fact]
        public void Parse_should_handle_named_colors()
        {
            ColorParser.Parse("maroon").ShouldBe(Color.Maroon);
        }

        [Fact]
        public void Parse_should_handle_RGB_colors()
        {
            ColorParser.Parse("#112233").ShouldBe(Color.FromArgb(0x11, 0x22, 0x33));
        }

        [Fact]
        public void Parse_should_handle_ARGB_colors()
        {
            ColorParser.Parse("#11223344").ShouldBe(Color.FromArgb(0x11, 0x22, 0x33, 0x44));
        }

        [Fact]
        public void Parse_should_handle_empty_strings()
        {
            ColorParser.Parse(" ").ShouldBe(Color.Empty);
        }

        [Fact]
        public void Parse_should_handle_transparent_color()
        {
            ColorParser.Parse("transparent").ShouldBe(Color.Transparent);
        }

        [Fact]
        public void Parse_should_be_case_and_whitespace_tolerant()
        {
            ColorParser.Parse(" rEd ").ShouldBe(Color.Red);
            ColorParser.Parse(" #ffFFff ").ShouldBe(Color.FromArgb(255, 255, 255));
        }

        [Theory]
        [InlineData("test")]
        [InlineData("#123")]
        public void Parse_should_throw_if_cannot_parse_color(string value)
        {
            Assert.Throws<ArgumentException>(() => ColorParser.Parse(value)).Message.ShouldStartWith($"Unable to parse Color: {value}");
        }
    }
}
