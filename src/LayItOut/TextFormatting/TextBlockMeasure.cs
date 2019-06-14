using System.Drawing;

namespace LayItOut.TextFormatting
{
    public struct TextBlockMeasure
    {
        public TextBlockMeasure(TextBlock block, SizeF measure, float spacePadding)
        {
            Block = block;
            Measure = measure;
            SpacePadding = spacePadding;
        }

        public TextBlock Block { get; }
        public SizeF Measure { get; }
        public float SpacePadding { get; }
    }
}