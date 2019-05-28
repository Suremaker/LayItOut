using System;
using LayItOut.Loaders;

namespace LayItOut
{
    public struct Alignment
    {
        public static readonly Alignment Center = new Alignment(VerticalAlignment.Center, HorizontalAlignment.Center);
        public static readonly Alignment TopLeft = new Alignment(VerticalAlignment.Top, HorizontalAlignment.Left);

        public HorizontalAlignment Horizontal { get; }
        public VerticalAlignment Vertical { get; }
        public Alignment(VerticalAlignment vertical) : this(vertical, default) { }
        public Alignment(HorizontalAlignment horizontal) : this(default, horizontal) { }
        public Alignment(VerticalAlignment vertical, HorizontalAlignment horizontal)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        public override string ToString()
        {
            if (Vertical == VerticalAlignment.Center && Horizontal == HorizontalAlignment.Center)
                return "Center";
            return $"{Vertical} {Horizontal}";
        }

        [AttributeParser]
        public static Alignment Parse(string value)
        {
            if (string.Equals(value, "center", StringComparison.OrdinalIgnoreCase))
                return new Alignment(VerticalAlignment.Center, HorizontalAlignment.Center);

            var split = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length != 2 || !Enum.TryParse<VerticalAlignment>(split[0], true, out var vertical) || !Enum.TryParse<HorizontalAlignment>(split[1], true, out var horizontal))
                throw new ArgumentException($"Provided value is not a valid {nameof(Alignment)}: {value}");

            return new Alignment(vertical, horizontal);
        }
    }
}