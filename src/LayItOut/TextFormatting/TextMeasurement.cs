using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LayItOut.Attributes;
using LayItOut.Rendering;

namespace LayItOut.TextFormatting
{
    public class TextMeasurement
    {
        public TextMeasure Measure(IRendererContext context, int maxWidth, params TextBlock[] blocks) => Measure(context, maxWidth, blocks.AsEnumerable());

        public TextMeasure Measure(IRendererContext context, int maxWidth, IEnumerable<TextBlock> blocks)
        {
            var results = new List<TextLineMeasure>();
            var line = new List<TextBlockMeasure>();
            float max = 0;

            float totalLength = 0;
            foreach (var area in ToBlockMeasure(context, blocks))
            {
                var space = line.Any() ? area.SpacePadding : 0;
                if (area.Block.IsLineBreak || totalLength + space + area.Measure.Width > maxWidth)
                {
                    max = Math.Max(max, totalLength);
                    var size = new SizeF(totalLength, GetHeight(line, area));

                    results.Add(new TextLineMeasure(size, line));
                    totalLength = 0;
                    space = 0;
                    line = new List<TextBlockMeasure>();
                }

                if (!area.Block.IsLineBreak)
                {
                    line.Add(area);
                    totalLength += area.Measure.Width + space;
                }
            }

            if (line.Any())
            {
                max = Math.Max(max, totalLength);
                var size = new SizeF(totalLength, GetHeight(line, line[0]));
                results.Add(new TextLineMeasure(size, line));
            }

            var measureWidth = (int)Math.Ceiling(max);
            var measureHeight = (int)Math.Ceiling(results.Aggregate(0f, (c, x) => c + x.Measure.Height));
            return new TextMeasure(new Size(measureWidth, measureHeight), results);
        }

        public TextLayout LayOut(int actualWidth, TextAlignment alignment, TextMeasure text)
        {
            var areas = new List<TextArea>();
            float y = 0;
            foreach (var line in text.Lines)
            {
                areas.AddRange(Arrange(line, y, actualWidth, alignment));
                y += line.Measure.Height;
            }

            return new TextLayout(areas);
        }


        private float GetHeight(List<TextBlockMeasure> line, TextBlockMeasure defaultMeasure)
        {
            var max = defaultMeasure;
            return line.Select(l => l).Aggregate(max, (current, f) => f.ActualMeasure.Height > current.ActualMeasure.Height ? f : current).ActualMeasure.Height;
        }

        private IEnumerable<TextArea> Arrange(TextLineMeasure line, float top, int actualWidth, TextAlignment alignment)
        {
            if (!line.Blocks.Any())
                yield break;
            var lineRemainingSpace = Math.Max(0, actualWidth - line.Measure.Width);
            var x = GetAlignmentOffset(lineRemainingSpace, alignment);
            var wordSpace = GetAlignmentWordSpace(line.Blocks, lineRemainingSpace, alignment);
            foreach (var block in line.Blocks)
            {
                var position = new PointF(x, top + (line.Measure.Height - block.Measure.Height) / 2);
                yield return new TextArea(position, block.Measure, block.Block, block.SpacePadding);
                x += block.Measure.Width + block.SpacePadding + wordSpace;
            }
        }

        private static float GetAlignmentWordSpace(IReadOnlyList<TextBlockMeasure> line, float lineRemainingSpace, TextAlignment alignment)
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

        private static IEnumerable<TextBlockMeasure> ToBlockMeasure(IRendererContext context, IEnumerable<TextBlock> textBlocks)
        {
            foreach (var block in textBlocks.SelectMany(b => b.Normalize()))
            {
                var text = block.Text;
                var size = context.MeasureText(text, block.Metadata.Font);
                var space = block.IsLineBreak ? 0f : context.GetSpaceWidth(block.Metadata.Font);
                yield return new TextBlockMeasure(block, size, space);
            }
        }
    }
}