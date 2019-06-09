using System.Drawing;
using System.Linq;
using LayItOut.Tests.TestHelpers;
using LayItOut.TextFormatting;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.TextFormatting
{
    public class TextMeasureTests
    {
        private readonly TextMeasure _measure = new TextMeasure(TestRenderingContext.Instance);
        private readonly Font _font = new Font(FontFamily.GenericMonospace, 10);

        [Fact]
        public void LayOut_should_put_blocks_in_same_line_if_they_fit()
        {
            var layout = _measure.LayOut(20, Block("Hello"), Block("Bob"), Block("my"), Block("friend!"));
            layout.Areas.Count.ShouldBe(4);
            layout.Areas.Select(x => x.Position).ShouldAllBe(x => x.Y == 0);
        }

        [Fact]
        public void LayOut_should_put_blocks_one_after_another_within_the_same_line_and_separate_by_space()
        {
            var layout = _measure.LayOut(20, Block("Hello"), Block("Bob"), Block("my"), Block("friend!"));
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
        public void LayOut_should_measure_all_blocks_sizes_and_space_padding()
        {
            var layout = _measure.LayOut(20, Block("Hello"), Block("Bob"), Block("my"), Block("friend!"));
            layout.Areas.Count.ShouldBe(4);
            foreach (var area in layout.Areas)
            {
                area.Size.ShouldBe(TestRenderingContext.Instance.MeasureText(area.Block.Text, area.Block.Metadata.Font));
                area.SpacePadding.ShouldBe(TestRenderingContext.Instance.GetSpaceWidth(area.Block.Metadata.Font));
            }
        }

        [Fact]
        public void LayOut_should_normalize_blocks()
        {
            var layout = _measure.LayOut(int.MaxValue, Block("How are you"), Block("Bob Smith", isInline: true), Block("my friend?"));
            layout.Areas.Select(x => x.Block.Text).ShouldBe(new[] { "How", "are", "you", "Bob Smith", "my", "friend?" });
        }

        [Fact]
        public void LayOut_should_move_block_to_new_line_if_does_not_fit()
        {
            var layout = _measure.LayOut(9, Block("Hello"), Block("Bob"), Block("my"), Block("friend!"));
            layout.Areas.Select(x => x.Position).ShouldBe(new[]
            {
                new PointF(0,0), new PointF(6,0),
                new PointF(0,_font.GetHeight()),
                new PointF(0,_font.GetHeight()*2)
            });
        }

        [Fact]
        public void LayOut_should_move_block_to_new_line_if_encounter_new_line_character()
        {
            var layout = _measure.LayOut(int.MaxValue, Block("Hello Bob,\nmy friend!"));
            layout.Areas.Select(x => x.Block.Text).ShouldBe(new[] { "Hello", "Bob,", "my", "friend!" });
            layout.Areas.Select(x => x.Position).ShouldBe(new[]
            {
                new PointF(0,0), new PointF(6,0),
                new PointF(0,_font.GetHeight()),
                new PointF(3,_font.GetHeight())
            });
        }

        //TODO: different font size tests
        //TODO: text alignment!
        private TextBlock Block(string text, bool isInline = false)
        {
            return new TextBlock(text, new TextMetadata(_font, Color.Blue), isInline);
        }
    }
}
