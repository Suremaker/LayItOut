using System;
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

        public TextLayout LayOut(int maxWidth, TextAlignment alignment, params TextBlock[] blocks) => LayOut(maxWidth, alignment, blocks.AsEnumerable());
        public TextLayout LayOut(int maxWidth, TextAlignment alignment, IEnumerable<TextBlock> blocks)
        {
            var lines = BuildLines(maxWidth, blocks, out var longestWidth);
            float y = 0;

            foreach (var line in lines)
                y += Arrange(line.line, y, line.size, longestWidth, alignment);

            return new TextLayout(lines.SelectMany(x => x.line).ToArray());
        }

        private List<(SizeF size, List<TextArea> line)> BuildLines(int maxWidth, IEnumerable<TextBlock> blocks, out int longestWidth)
        {
            var results = new List<(SizeF size, List<TextArea> line)>();
            var line = new List<TextArea>();
            float max = 0;

            float totalLength = 0;
            foreach (var area in blocks.SelectMany(Split))
            {
                var space = line.Any() ? area.SpacePadding : 0;
                if (area.Block.IsLineBreak || totalLength + space + area.Size.Width > maxWidth)
                {
                    max = Math.Max(max, totalLength);
                    var size = new SizeF(totalLength, GetHeight(line, area));
                    results.Add((size, line));
                    totalLength = 0;
                    line = new List<TextArea>();
                }

                if (!area.Block.IsLineBreak)
                {
                    line.Add(area);
                    totalLength += area.Size.Width + space;
                }
            }

            if (line.Any())
            {
                max = Math.Max(max, totalLength);
                var size = new SizeF(totalLength, GetHeight(line, line[0]));
                results.Add((size, line));
            }

            longestWidth = (int)Math.Ceiling(max);
            return results;
        }

        private float GetHeight(List<TextArea> line, TextArea defaultArea)
        {
            if (!line.Any()) return 0;
            var max = defaultArea.Block.Metadata.Font;
            max = line.Select(l => l.Block.Metadata.Font).Aggregate(max, (current, f) => f.Size > current.Size ? f : current);
            return max.GetHeight();
        }

        private float Arrange(List<TextArea> line, float top, SizeF lineSize, int maxWidth, TextAlignment alignment)
        {
            if (!line.Any())
                return lineSize.Height;
            var lineRemainingSpace = Math.Max(0, maxWidth - lineSize.Width);
            var x = GetAlignmentOffset(lineRemainingSpace, alignment);
            var wordSpace = GetAlignmentWordSpace(line, lineRemainingSpace, alignment);
            foreach (var area in line)
            {
                area.Position = new PointF(x, top + (lineSize.Height - area.Size.Height) / 2);
                x += area.Size.Width + area.SpacePadding + wordSpace;
            }

            return lineSize.Height;
        }

        private static float GetAlignmentWordSpace(List<TextArea> line, float lineRemainingSpace, TextAlignment alignment)
        {
            if (line.Count < 2 || alignment != TextAlignment.Justify)
                return 0;
            return lineRemainingSpace / (line.Count - 1);
        }

        private float GetAlignmentOffset(float lineRemainingSpace, TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.Right:
                    return lineRemainingSpace;
                case TextAlignment.Center:
                    return (lineRemainingSpace) * 0.5f;
                default:
                    return 0;
            }
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