using System;
using System.Drawing;

namespace LayItOut
{
    public struct BorderLine
    {
        public static readonly BorderLine NotSet = new BorderLine();
        public SizeUnit Size { get; }
        public Color Color { get; }

        public BorderLine(SizeUnit size, Color color)
        {
            Size = size;
            Color = color;
        }

        public static BorderLine Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return NotSet;

            var parts = value.Trim().Split(' ');
            try
            {
                if (parts.Length == 2)
                    return new BorderLine(SizeUnit.Parse(parts[0]), ColorParser.Parse(parts[1]));
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Provided value is not a valid {nameof(BorderLine)}: {value}", nameof(value), e);
            }
            throw new ArgumentException($"Provided value is not a valid {nameof(BorderLine)}: {value}", nameof(value));
        }

        public override string ToString()
        {
            return $"{Size} {Color}";
        }
    }
}