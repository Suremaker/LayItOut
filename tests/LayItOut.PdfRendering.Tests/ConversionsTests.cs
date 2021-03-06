﻿using System;
using LayItOut.Attributes;
using PdfSharp.Drawing;
using Shouldly;
using Xunit;

namespace LayItOut.PdfRendering.Tests
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
            Enum.TryParse<XFontStyle>(name, true, out var expected).ShouldBe(true);
            style.ToXFontStyle().ShouldBe(expected);
        }
    }
}
