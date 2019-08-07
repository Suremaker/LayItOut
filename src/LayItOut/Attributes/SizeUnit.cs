using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using LayItOut.Loaders;

namespace LayItOut.Attributes
{
    [Description("Specifies size that can be absolute, not defined or unlimited")]
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
                return "-";
            if (Mode == SizeMode.Unlimited)
                return "*";
            return $"{Value}";
        }

        public static implicit operator SizeUnit(int value) => Absolute(value);

        [Pure]
        public int AbsoluteOrDefault(int defaultValue = 0) => IsAbsolute ? Value : defaultValue;

        [Description("* `-` - not set\n* `*` - take all free space\n* `[absolute]` - absolute value (integer)")]
        [AttributeParser]
        public static SizeUnit Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "-")
                return NotSet;

            if (value == "*")
                return Unlimited;

            return int.TryParse(value, out var absolute)
                ? Absolute(absolute)
                : throw new ArgumentException($"Provided value is not a valid {nameof(SizeUnit)}: {value}", nameof(value));
        }

        public int GetValue(Func<int> unlimitedValueFn, int notSetValue = 0)
        {
            switch (Mode)
            {
                case SizeMode.NotSet:
                    return notSetValue;
                case SizeMode.Unlimited:
                    return unlimitedValueFn();
                case SizeMode.Absolute:
                    return Value;
                default:
                    throw new NotSupportedException($"{Mode} is not supported.");
            }
        }

        public int GetValue(int unlimitedValueFn, int notSetValue = 0)
        {
            switch (Mode)
            {
                case SizeMode.NotSet:
                    return notSetValue;
                case SizeMode.Unlimited:
                    return unlimitedValueFn;
                case SizeMode.Absolute:
                    return Value;
                default:
                    throw new NotSupportedException($"{Mode} is not supported.");
            }
        }

        public static bool operator ==(SizeUnit x, SizeUnit y) => x.Equals(y);
        public static bool operator !=(SizeUnit x, SizeUnit y) => !x.Equals(y);
        public bool Equals(SizeUnit other) => Value == other.Value && Mode == other.Mode;
        public override bool Equals(object obj) => obj is SizeUnit other && Equals(other);
        public override int GetHashCode()
        {
            unchecked
            {
                return (Value * 397) ^ (int)Mode;
            }
        }

        public static int[] Distribute(int size, int parts)
        {
            if (parts <= 0)
                return Array.Empty<int>();
            var result = new int[parts];
            float accumulated = 0;
            int total = 0;
            var step = size / (float)parts;

            for (var i = 0; i < parts; ++i)
            {
                accumulated += step;
                var integer = (int)accumulated;
                result[i] = integer;
                total += integer;
                accumulated -= integer;
            }

            result[parts - 1] += size - total;
            return result;
        }
    }
}