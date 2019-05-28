using System;
using System.Linq;
using LayItOut.Loaders;

namespace LayItOut
{
    public struct Border
    {
        public static readonly Border None = new Border();
        public BorderLine Top { get; }
        public BorderLine Left { get; }
        public BorderLine Bottom { get; }
        public BorderLine Right { get; }

        public Spacer AsSpacer() => new Spacer(Top.Size, Left.Size, Bottom.Size, Right.Size);

        public Border(BorderLine top, BorderLine left, BorderLine bottom, BorderLine right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public Border(BorderLine vertical, BorderLine horizontal) : this(vertical, horizontal, vertical, horizontal)
        {

        }

        public Border(BorderLine line) : this(line, line)
        {
        }

        [AttributeParser]
        public static Border Parse(string value)
        {
            var parts = value.Split(';').Select(BorderLine.Parse).ToArray();
            if (parts.Length == 4)
                return new Border(parts[0], parts[1], parts[2], parts[3]);
            if (parts.Length == 2)
                return new Border(parts[0], parts[1]);
            if (parts.Length == 1)
                return new Border(parts[0]);
            throw new ArgumentException($"Provided value is not a valid border: {value}", nameof(value));
        }

        public override string ToString()
        {
            return $"{Top};{Left};{Bottom};{Right}";
        }
    }
}
