using System;
using System.Drawing;
using System.Linq;

namespace LayItOut
{
    public struct Spacer
    {
        public static readonly Spacer None = new Spacer();

        public SizeUnit Top { get; }
        public SizeUnit Left { get; }
        public SizeUnit Bottom { get; }
        public SizeUnit Right { get; }

        public Spacer(SizeUnit top, SizeUnit left, SizeUnit bottom, SizeUnit right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public Spacer(SizeUnit vertical, SizeUnit horizontal) : this(vertical, horizontal, vertical, horizontal)
        {

        }

        public Spacer(SizeUnit distance) : this(distance, distance)
        {
        }

        public static Spacer Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return None;

            try
            {
                var parts = value.Trim().Split(' ').Select(SizeUnit.Parse).ToArray();
                if (parts.Length == 4)
                    return new Spacer(parts[0], parts[1], parts[2], parts[3]);
                if (parts.Length == 2)
                    return new Spacer(parts[0], parts[1]);
                if (parts.Length == 1)
                    return new Spacer(parts[0]);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Provided value is not a valid {nameof(Spacer)}: {value}", nameof(value), e);
            }
            throw new ArgumentException($"Provided value is not a valid {nameof(Spacer)}: {value}", nameof(value));
        }
        public override string ToString()
        {
            return $"{Top} {Left} {Bottom} {Right}";
        }

        public Size GetAbsoluteSize() => new Size(Left.AbsoluteOrDefault() + Right.AbsoluteOrDefault(), Top.AbsoluteOrDefault() + Bottom.AbsoluteOrDefault());
    }
}
