using System.Drawing;
using System.Linq;
using LayItOut.TextFormatting;
using Shouldly;
using Xunit;

namespace LayItOut.Tests.TextFormatting
{
    public class TextBlockTests
    {
        private readonly ITextMetadata _meta;

        public TextBlockTests()
        {
            var font = new FontInfo("TestFamily", 14);
            _meta = new TextMetadata(font, Color.AliceBlue, 1);
        }

        [Theory]
        [InlineData("Hello world!?", true, true)]
        [InlineData("Hello world!?", false, false)]
        [InlineData("Hello\tworld!?", false, false)]
        [InlineData("Hello\tworld!?", true, false)]
        [InlineData("Hello\nworld!?", false, false)]
        [InlineData("Hello\nworld!?", true, false)]
        [InlineData("Hello\rworld!?", false, false)]
        [InlineData("Hello\rworld!?", true, false)]
        public void Should_determine_if_text_is_normalized(string text, bool isInline, bool expectedIsNormalized)
        {
            new TextBlock(text, _meta, isInline).IsNormalized.ShouldBe(expectedIsNormalized);
        }

        [Theory]
        [InlineData("\r\t \nHello my friend!\r\nHow\tare\ryou?\n\r\t ", false, "\n", "Hello", "my", "friend!", "\n", "How", "are", "you?", "\n")]
        [InlineData("\r\t Hello my friend!\r\nHow\tare\ryou?\r\t ", true, "Hello my friend! How are you?")]
        public void Normalize_should_split_blocks_and_normalize_text(string text, bool isInline, params string[] expectedBlocks)
        {
            var blocks = new TextBlock(text, _meta, isInline)
                .Normalize()
                .ToArray();

            blocks.Select(b => b.Text).ShouldBe(expectedBlocks);
            blocks.ShouldAllBe(x => x.IsNormalized);
            blocks.ShouldAllBe(x => x.IsInline);
            blocks.ShouldAllBe(x => x.Metadata == _meta);
        }

        [Fact]
        public void Normalize_of_normalized_block_should_return_self()
        {
            var original = new TextBlock("Hi!\r", _meta, true);
            var normalized = original.Normalize().Single();

            normalized.ShouldNotBeSameAs(original);
            normalized.Normalize().Single().ShouldBeSameAs(normalized);
        }

        [Fact]
        public void IsLineBreak_should_identify_line_breaks()
        {
            var blocks = new TextBlock("Hi\nBob", _meta, false).Normalize().ToArray();
            blocks.Select(b => b.Text).ShouldBe(new[] { "Hi", "\n", "Bob" });
            blocks.Select(b => b.IsLineBreak).ShouldBe(new[] { false, true, false });
        }
    }
}