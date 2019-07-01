using System;
using System.Drawing;
using Shouldly;
using Xunit;

namespace LayItOut.BitmapRendering.Tests
{
    public class ConversionsTests
    {
        [Theory]
        [InlineData(FontInfoStyle.Bold)]
        [InlineData(FontInfoStyle.Italic)]
        [InlineData(FontInfoStyle.Regular)]
        [InlineData(FontInfoStyle.Strikeout)]
        [InlineData(FontInfoStyle.Underline)]
        public void It_should_properly_convert_style(FontInfoStyle style)
        {
            var name = Enum.GetName(typeof(FontInfoStyle), style);
            Enum.TryParse<FontStyle>(name, true, out var expected).ShouldBe(true);
            style.ToFontStyle().ShouldBe(expected);
        }
    }
}