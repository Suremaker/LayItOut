using Shouldly;
using Xunit;

namespace LayItOut.Tests.Loaders
{
    public class FontInfoTests
    {
        [Theory]
        [InlineData("Arial;10", "Arial", 10, FontInfoStyle.Regular)]
        [InlineData("Arial;10;bold,underline", "Arial", 10, FontInfoStyle.Bold | FontInfoStyle.Underline)]
        public void It_should_parse_font(string font, string expectedFamily, float expectedSize, FontInfoStyle expectedStyle)
        {
            var f = FontInfo.Parse(font);
            f.ShouldNotBeNull();
            f.Family.ShouldBe(expectedFamily);
            f.Size.ShouldBe(expectedSize);
            f.Style.ShouldBe(expectedStyle);
        }

        [Fact]
        public void Default_font_should_match_none()
        {
            new FontInfo().ShouldBe(FontInfo.None);
        }
    }
}
