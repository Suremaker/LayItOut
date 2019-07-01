using System;

namespace LayItOut
{
    [Flags]
    public enum FontInfoStyle
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        Strikeout = 8
    }
}