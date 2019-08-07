using System;
using System.ComponentModel;

namespace LayItOut.Attributes
{
    [Description("Asset source.")]
    public struct AssetSource
    {
        public static readonly AssetSource None = new AssetSource();
        public string Location { get; }
        public byte[] Content { get; }
        public bool IsCached { get; }
        public bool IsNone => Content == null;

        public AssetSource(string location, byte[] content, bool isCached)
        {
            Location = location;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            IsCached = isCached;
        }

        public override string ToString()
        {
            if (IsNone)
                return "-none-";
            return Location ?? "-inline-";
        }
    }
}