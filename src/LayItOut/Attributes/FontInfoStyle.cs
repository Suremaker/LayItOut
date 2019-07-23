using System;
using System.ComponentModel;

namespace LayItOut.Attributes
{
    [Description("Font style where values can be combined.")]
    [Flags]
    public enum FontInfoStyle : byte
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        Strikeout = 8
    }
}