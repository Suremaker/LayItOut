using System.Collections.Generic;
using System.Drawing;

namespace LayItOut.TextFormatting
{
    public struct TextMeasure
    {
        public Size Measure { get; }
        public IReadOnlyList<TextLineMeasure> Lines { get; }

        public TextMeasure(Size measure, IReadOnlyList<TextLineMeasure> lines)
        {
            Measure = measure;
            Lines = lines;
        }
    }
}