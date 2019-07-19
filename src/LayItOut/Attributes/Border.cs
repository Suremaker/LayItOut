using System;
using System.ComponentModel;
using System.Drawing;
using LayItOut.Loaders;

namespace LayItOut.Attributes
{
    [Description("Specifies border with size and color.")]
    public struct Border
    {
        public static readonly Border NotSet = new Border();
        public int Size { get; }
        public Color Color { get; }

        public Border(int size, Color color)
        {
            Size = size;
            Color = color;
        }

        public Spacer AsSpacer() => new Spacer(Size);

        [Description("`[size] [color]` (size as integer, color by name, #RRGGBB or #AARRGGBB format)")]
        [AttributeParser]
        public static Border Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return NotSet;

            var parts = value.Trim().Split(' ');
            try
            {
                if (parts.Length == 2)
                    return new Border(int.Parse(parts[0]), ColorParser.Parse(parts[1]));
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Provided value is not a valid {nameof(Border)}: {value}", nameof(value), e);
            }
            throw new ArgumentException($"Provided value is not a valid {nameof(Border)}: {value}", nameof(value));
        }

        public override string ToString()
        {
            return $"{Size} {Color}";
        }
    }
}