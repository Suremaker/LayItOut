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
        public SizeF ActualMeasure => new SizeF(Measure.Width, Measure.Height * Block.Metadata.LineHeight);
        public float SpacePadding { get; }
    }
}