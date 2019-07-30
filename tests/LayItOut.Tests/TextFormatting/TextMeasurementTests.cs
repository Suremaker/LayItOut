using System.Drawing;
using System.Linq;
using LayItOut.Attributes;
using LayItOut.Tests.TestHelpers;
using LayItOut.TextFormatting;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.TextFormatting
{
    public class TextMeasurementTests
    {
        private readonly TextMeasurement _measurement = new TextMeasurement();
        private readonly FontInfo _font = new FontInfo("Mono", 10);

        [Fact]
        public void Measure_should_put_blocks_in_same_line_if_they_fit()
        {
            var measure = _measurement.Measure(TestRendererContext.Instance, 20, Block("Hello"), Block("Bob"), Block("my"), Block("friend!"));
            measure.Lines.Count.ShouldBe(1);
            measure.Lines[0].Blocks.Count.ShouldBe(4);
        }

        [Fact]
        public void Measure_should_measure_all_blocks_sizes_and_space_padding()
        {
            var measure = _measurement.Measure(TestRendererContext.Instance, 20, Block("Hello"), Block("Bob"), Block("my"), Block("friend!"));
            foreach (var area in measure.Lines.SelectMany(x => x.Blocks))
            {
                area.Measure.ShouldBe(TestRendererContext.Instance.MeasureText(area.Block.Text, area.Block.Metadata.Font));
                area.SpacePadding.ShouldBe(TestRendererContext.Instance.GetSpaceWidth(area.Block.Metadata.Font));
            }
        }

        [Fact]
        public void Measure_should_honor_text_continuation()
        {
            var measure = _measurement.Measure(TestRendererContext.Instance, int.MaxValue,
                Block("Hell"),
                Block("o", isContinuation: true),
                Block("Bob"));

            var line = measure.Lines.Single();
            line.Blocks[0].SpacePadding.ShouldBe(TestRendererContext.Instance.GetSpaceWidth(_font));
            line.Blocks[1].SpacePadding.ShouldBe(0);
            line.Blocks[2].SpacePadding.ShouldBe(TestRendererContext.Instance.GetSpaceWidth(_font));

            line.Measure.Width.ShouldBe(line.Blocks.Sum(x => x.Measure.Width) + line.Blocks[2].SpacePadding);
        }

        [Fact]
        public void Measure_should_preserve_empty_line_height()
        {
            var measure = _measurement.Measure(TestRendererContext.Instance, 20, Block("Hey\n\nbob!"));
            measure.Lines.Select(x => x.Blocks.Count).ShouldBe(new[] { 1, 0, 1 });
            measure.Lines[1].Measure.ShouldBe(new SizeF(0, TestRendererContext.Instance.GetHeight(_font)));
        }

        [Fact]
        public void Measure_should_normalize_blocks()
        {
            var measure = _measurement.Measure(TestRendererContext.Instance, int.MaxValue, Block("How are you"), Block("Bob Smith", isInline: true), Block("my friend?"));
            measure.Lines.SelectMany(x => x.Blocks.Select(b => b.Block.Text)).ShouldBe(new[] { "How", "are", "you", "Bob Smith", "my", "friend?" });
        }

        [Fact]
        public void Measure_should_move_block_to_new_line_if_does_not_fit()
        {
            var measure = _measurement.Measure(TestRendererContext.Instance, 9, Block("Hello"), Block("Bob"), Block("my"), Block("friend!"));
            measure.Lines.Select(x => x.Blocks.Count).ShouldBe(new[] { 2, 1, 1 });
        }

        [Fact]
        public void Measure_should_move_block_to_new_line_if_encounter_new_line_character()
        {
            var measure = _measurement.Measure(TestRendererContext.Instance, int.MaxValue, Block("Hello Bob,\nmy friend!"));
            measure.Lines.SelectMany(x => x.Blocks.Select(b => b.Block.Text)).ShouldBe(new[] { "Hello", "Bob,", "my", "friend!" });
            measure.Lines.Select(x => x.Blocks.Count).ShouldBe(new[] { 2, 2 });
        }

        [Theory]
        [InlineData(TextAlignment.Left, "Hello my friend!\nHow are you?", 20, 16, 0f, 6f, 9f, 0f, 4f, 8f)]
        [InlineData(TextAlignment.Right, "Hello my friend!\nHow are you?", 20, 20, 4f, 10f, 13f, 8f, 12f, 16f)]
        [InlineData(TextAlignment.Center, "Hello my friend!\nHow are you?", 20, 18, 2f, 8f, 11f, 4f, 8f, 12f)]
        [InlineData(TextAlignment.Justify, "Hello my friend!\nHow are you?", 20, 20, 0f, 8f, 13f, 0f, 8f, 16f)]
        [InlineData(TextAlignment.Justify, "Hello", 20, 5, 0f)]
        public void LayOut_should_align_text_accordingly(TextAlignment alignment, string text, int width, int expectedWidth, params float[] positions)
        {
            var layout = _measurement.LayOut(width, alignment, _measurement.Measure(TestRendererContext.Instance, width, Block(text)));
            layout.Size.Width.ShouldBe(expectedWidth);
            layout.Areas.Select(a => a.Position.X).ShouldBe(positions);
        }

        [Theory]
        [InlineData(TextAlignment.Left, 20, 15, 0f, 1f, 4f, 6f, 9f)]
        [InlineData(TextAlignment.Right, 20, 20, 5f, 6f, 9f, 11f, 14f)]
        [InlineData(TextAlignment.Center, 20, 18, 2.5f, 3.5f, 6.5f, 8.5f, 11.5f)]
        [InlineData(TextAlignment.Justify, 20, 20, 0f, 1f, 4f, 8.5f, 14f)]
        public void LayOut_should_align_continued_text_accordingly(TextAlignment alignment, int width, int expectedWidth, params float[] positions)
        {
            var layout = _measurement.LayOut(width, alignment, _measurement.Measure(TestRendererContext.Instance, width,
                Block("H"), Block("ell", isContinuation: true), Block("o", isContinuation: true), Block("my friend")));
            layout.Size.Width.ShouldBe(expectedWidth);
            layout.Areas.Select(a => a.Position.X).ShouldBe(positions);
        }

        [Fact]
        public void LayOut_should_put_blocks_one_after_another_within_the_same_line_and_separate_by_space()
        {
            var measure = _measurement.Measure(TestRendererContext.Instance, 20, Block("Hello"), Block("Bob"), Block("my"), Block("friend!"));
            var layout = _measurement.LayOut(20, TextAlignment.Left, measure);
            layout.Areas.Count.ShouldBe(4);
            var last = layout.Areas[0];
            last.Position.X.ShouldBe(0);
            foreach (var current in layout.Areas.Skip(1))
            {
                current.Position.X.ShouldBe(last.Area.Right + current.SpacePadding);
                last = current;
            }
        }

        [Fact]
        public void LayOut_should_exclude_white_space_on_word_wrap()
        {
            var measure = _measurement.Measure(TestRendererContext.Instance, 10, Block("Hello Bob my friend"));
            var layout = _measurement.LayOut(10, TextAlignment.Right, measure);
            layout.Areas.Select(a => a.Position).ShouldBe(new[]
            {
                new PointF(1,0),
                new PointF(7,0),
                new PointF(1,TestRendererContext.Instance.GetHeight(_font)),
                new PointF(4,TestRendererContext.Instance.GetHeight(_font))
            });
        }

        [Fact]
        public void LayOut_should_honor_line_height()
        {
            var lineHeight = 0.5f;
            var origHeight = TestRendererContext.Instance.GetHeight(_font);
            var measure = _measurement.Measure(TestRendererContext.Instance, 10, Block("Hello Bob my friend", lineHeight));
            var layout = _measurement.LayOut(10, TextAlignment.Right, measure);
            layout.Areas.Select(a => a.Position).ShouldBe(new[]
            {
                new PointF(1,-origHeight*lineHeight/2),
                new PointF(7,-origHeight*lineHeight/2),
                new PointF(1,origHeight*lineHeight/2),
                new PointF(4,origHeight*lineHeight/2)
            });
        }

        private TextBlock Block(string text, float lineHeight = 1, bool isInline = false, bool isContinuation = false)
        {
            return new TextBlock(text, new TextMetadata(_font, Color.Blue, lineHeight), isInline, isContinuation);
        }
    }
}
