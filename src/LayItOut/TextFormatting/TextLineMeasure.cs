using System.Collections.Generic;
using System.Drawing;

namespace LayItOut.TextFormatting
{
    public struct TextLineMeasure
    {
        public SizeF Measure { get; }
        public IReadOnlyList<TextBlockMeasure> Blocks { get; }

        public TextLineMeasure(SizeF measure,IReadOnlyList<TextBlockMeasure> blocks)
        {
            Measure = measure;
            Blocks = blocks;
        }
    }
}