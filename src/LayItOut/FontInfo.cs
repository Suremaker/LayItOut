using System;
using LayItOut.Loaders;

namespace LayItOut
{
    public struct FontInfo : IEquatable<FontInfo>
    {
        public static readonly FontInfo None = new FontInfo();

        public FontInfo(string family, float size, FontInfoStyle style = FontInfoStyle.Regular)
        {
            Family = family;
            Size = size;
            Style = style;
        }

        public string Family { get; }
        public float Size { get; }
        public FontInfoStyle Style { get; }

        [AttributeParser]
        public static FontInfo Parse(string value)
        {
            var parts = value.Split(';');
            var style = FontInfoStyle.Regular;
            if (parts.Length < 2 || !float.TryParse(parts[1], out var size)
                                 || (parts.Length == 3 && !Enum.TryParse(parts[2], true, out style))
                                 || parts.Length > 3)
            {
                throw new ArgumentException($"Unable to parse {nameof(FontInfo)}: {value}", nameof(value));
            }

            try
            {
                return new FontInfo(parts[0], size, style);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Unable to parse {nameof(FontInfo)}: {value} - {e.Message}", nameof(value), e);
            }
        }

        public override string ToString()
        {
            if (Size <= 0)
                return "<none>";
            return $"{Family} {Size} {Style}";
        }

        public bool Equals(FontInfo other)
        {
            return string.Equals(Family, other.Family) && Size.Equals(other.Size) && Style == other.Style;
        }

        public override bool Equals(object obj)
        {
            return obj is FontInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Family != null ? Family.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Size.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)Style;
                return hashCode;
            }
        }

        public static bool operator ==(FontInfo left, FontInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FontInfo left, FontInfo right)
        {
            return !left.Equals(right);
        }
    }
}
