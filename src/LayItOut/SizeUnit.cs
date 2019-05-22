using System.Diagnostics.Contracts;

namespace LayItOut
{
    public struct SizeUnit
    {
        private enum SizeMode : byte
        {
            NotSet,
            Absolute,
            Unlimited
        }
        public static readonly SizeUnit NotSet = new SizeUnit(0, SizeMode.NotSet);
        public static readonly SizeUnit Zero = new SizeUnit(0, SizeMode.Absolute);
        public static readonly SizeUnit Unlimited = new SizeUnit(0, SizeMode.Unlimited);
        public int Value { get; }
        private SizeMode Mode { get; }
        public bool IsAbsolute => Mode == SizeMode.Absolute;
        public bool IsUnlimited => Mode == SizeMode.Unlimited;
        public bool IsNotSet => Mode == SizeMode.NotSet;

        private SizeUnit(int value, SizeMode mode)
        {
            Value = value;
            Mode = mode;
        }

        public static SizeUnit Absolute(int value) => new SizeUnit(value, SizeMode.Absolute);

        public override string ToString()
        {
            if (Mode == SizeMode.NotSet)
                return "not set";
            if (Mode == SizeMode.Unlimited)
                return "*";
            return $"{Value}";
        }

        public static implicit operator SizeUnit(int value) => Absolute(value);

        [Pure]
        public int ApplyIfSet(int value) => IsAbsolute ? Value : value;
    }
}