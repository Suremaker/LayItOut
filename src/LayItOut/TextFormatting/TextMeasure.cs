using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LayItOut.Rendering;

namespace LayItOut.TextFormatting
{
    public class TextMeasure
    {
        private readonly IRenderingContext _context;

        public TextMeasure(IRenderingContext context) => _context = context;

        public TextLayout LayOut(int maxWidth, params TextBlock[] blocks)
        {
            var results = new List<TextArea>();
            var line = new List<TextArea>();
            float totalLength = 0;
            float y = 0;
            foreach (var area in blocks.SelectMany(Split))
            {
                var space = line.Any() ? area.SpacePadding : 0;
                if (area.Block.IsLineBreak || totalLength + space + area.Size.Width > maxWidth)
                {
                    y += Arrange(line, y, totalLength, maxWidth, GetBiggestFont(line, area.Block.Metadata.Font));
                    totalLength = 0;
                    results.AddRange(line);
                    line.Clear();
                }
                if (!area.Block.IsLineBreak)
                {
                    line.Add(area);
                    totalLength += area.Size.Width + space;
                }
            }

            if (line.Any())
            {
                Arrange(line, y, totalLength, maxWidth, GetBiggestFont(line, line.First().Block.Metadata.Font));
                results.AddRange(line);
            }

            return new TextLayout(results);
        }

        private Font GetBiggestFont(List<TextArea> line, Font defaultFont) => line.Select(l=>l.Block.Metadata).Aggregate(defaultFont, (c, m) => c.Size > m.Font.Size ? c : m.Font);

        private float Arrange(List<TextArea> line, float top, float totalLength, int maxWidth, Font lineFont)
        {
            var height = lineFont.GetHeight();
            float x = 0;
            foreach (var area in line)
            {
                area.Position = new PointF(x, top + (height - area.Size.Height) / 2);
                x += area.Size.Width + area.SpacePadding;
            }

            return height;
        }

        private IEnumerable<TextArea> Split(TextBlock block) => block.Normalize().Select(ToTextArea);

        private TextArea ToTextArea(TextBlock block)
        {
            var text = block.Text;
            var size = block.IsLineBreak ? SizeF.Empty : _context.MeasureText(text, block.Metadata.Font);
            var space = block.IsLineBreak ? 0f : _context.GetSpaceWidth(block.Metadata.Font);
            return new TextArea(size, block, space);
        }
    }
}