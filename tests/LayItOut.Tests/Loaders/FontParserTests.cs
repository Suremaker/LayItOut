using System;
using System.Drawing;
using LayItOut.Loaders;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.Loaders
{
    public class FontParserTests : IDisposable
    {
        private readonly FontParser _parser = new FontParser();
        
        [Theory]
        [InlineData("Arial;10", "Arial", 10, FontStyle.Regular)]
        [InlineData("Arial;10;bold,underline", "Arial", 10, FontStyle.Bold|FontStyle.Underline)]
        public void It_should_parse_font(string font, string expectedFamily, float expectedSize, FontStyle expectedStyle)
        {
            var f = _parser.Parse(font);
            f.ShouldNotBeNull();
            f.FontFamily.Name.ShouldBe(expectedFamily);
            f.Size.ShouldBe(expectedSize);
            f.Style.ShouldBe(expectedStyle);
            f.Unit.ShouldBe(GraphicsUnit.World);
        }

        [Fact]
        public void It_should_parse_custom_font()
        {
            _parser.AddFont($"{AppContext.BaseDirectory}\\Loaders\\ahronbd.ttf");
            _parser.Parse("Aharoni;10").FontFamily.Name.ShouldBe("Aharoni");
        }

        public void Dispose()
        {
            _parser.Dispose();
        }
    }
}
