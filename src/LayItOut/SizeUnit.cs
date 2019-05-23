using System;
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

        public static SizeUnit Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return NotSet;

            if (value == "*")
                return Unlimited;

            return int.TryParse(value, out var absolute)
                ? Absolute(absolute)
                : throw new ArgumentException($"Provided value is not a valid {nameof(SizeUnit)}: {value}", nameof(value));
        }

        public static bool operator ==(SizeUnit x, SizeUnit y) => x.Equals(y);
        public static bool operator !=(SizeUnit x, SizeUnit y) => !x.Equals(y);
        public bool Equals(SizeUnit other) => Value == other.Value && Mode == other.Mode;
        public override bool Equals(object obj) => obj is SizeUnit other && Equals(other);
        public override int GetHashCode()
        {
            unchecked
            {
                return (Value * 397) ^ (int) Mode;
            }
        }
    }
}