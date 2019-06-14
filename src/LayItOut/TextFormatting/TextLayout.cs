using System;
using System.Collections.Generic;
using System.Drawing;

namespace LayItOut.TextFormatting
{
    public class TextLayout
    {
        public Size Size { get; }
        public IReadOnlyList<TextArea> Areas { get; }

        public TextLayout(IReadOnlyList<TextArea> areas)
        {
            Areas = areas;
            float width = 0, height = 0;
            foreach (var area in areas)
            {
                width = Math.Max(width, area.Area.Right);
                height = Math.Max(height, area.Area.Bottom);
            }

            Size = new Size((int)Math.Ceiling(width), (int)Math.Ceiling(height));
        }
    }
}