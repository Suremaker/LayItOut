using System;
using System.Drawing;
using System.Globalization;

namespace LayItOut
{
    public static class ColorParser
    {
        public static Color Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Color.Empty;

            value = value.Trim();

            var color = value.StartsWith("#")
                ? ParseArgb(value)
                : ParseKnown(value);

            return !color.IsEmpty ? color : throw new ArgumentException($"Unable to parse {nameof(Color)}: {value}", nameof(value));
        }

        private static Color ParseKnown(string value)
        {
            var known = Color.FromName(value);
            return known.ToArgb() != 0 ? known : Color.Empty;
        }

        private static Color ParseArgb(string value)
        {
            if (value.Length == 7)
                return Color.FromArgb(
                    int.Parse(value.Substring(1, 2), NumberStyles.AllowHexSpecifier),
                    int.Parse(value.Substring(3, 2), NumberStyles.AllowHexSpecifier),
                    int.Parse(value.Substring(5, 2), NumberStyles.AllowHexSpecifier));
            if (value.Length == 9)
                return Color.FromArgb(
                    int.Parse(value.Substring(1, 2), NumberStyles.AllowHexSpecifier),
                    int.Parse(value.Substring(3, 2), NumberStyles.AllowHexSpecifier),
                    int.Parse(value.Substring(5, 2), NumberStyles.AllowHexSpecifier),
                    int.Parse(value.Substring(7, 2), NumberStyles.AllowHexSpecifier));
            return Color.Empty;
        }
    }
}