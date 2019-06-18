using System;
using System.Linq;
using LayItOut.Loaders;

namespace LayItOut
{
    public struct BorderRadius
    {
        public static readonly BorderRadius None = new BorderRadius();

        public float TopLeft { get; }
        public float TopRight { get; }
        public float BottomLeft { get; }
        public float BottomRight { get; }

        public BorderRadius(float topLeft, float topRight, float bottomRight, float bottomLeft)
        {
            TopLeft = topLeft;
            TopRight = topRight;
            BottomLeft = bottomLeft;
            BottomRight = bottomRight;
        }

        public BorderRadius(float top, float bottom) : this(top, top, bottom, bottom) { }
        public BorderRadius(float rounding) : this(rounding, rounding) { }

        [AttributeParser]
        public static BorderRadius Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return None;

            try
            {
                var parts = value.Trim().Split(' ').Select(float.Parse).ToArray();
                if (parts.Length == 4)
                    return new BorderRadius(parts[0], parts[1], parts[2], parts[3]);
                if (parts.Length == 2)
                    return new BorderRadius(parts[0], parts[1]);
                if (parts.Length == 1)
                    return new BorderRadius(parts[0]);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Provided value is not a valid {nameof(BorderRadius)}: {value}", nameof(value), e);
            }
            throw new ArgumentException($"Provided value is not a valid {nameof(BorderRadius)}: {value}", nameof(value));
        }

        public override string ToString()
        {
            return $"{TopLeft} {TopRight} {BottomRight} {BottomLeft}";
        }
    }
}