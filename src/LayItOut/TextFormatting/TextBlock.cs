using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LayItOut.TextFormatting
{
    public class TextBlock
    {
        private const string LineBreak = "\n";
        private static readonly char[] CharsToNormalize = new[] { '\r', '\t' };
        private static readonly char[] InlineCharsToNormalize = new[] { '\r', '\n', '\t' };
        private static readonly char[] CharsToBreakOn = new[] { ' ', '\r', '\n', '\t' };
        public Font Font { get; }
        public Color Color { get; }
        public string Text { get; }
        public bool IsInline { get; }
        public bool IsNormalized { get; }
        public bool IsLineBreak => Text == LineBreak;

        public TextBlock(Font font, string text, Color color, bool isInline) : this(font, text, color, isInline, IsTextNormalized(isInline, text)) { }

        private TextBlock(Font font, string text, Color color, bool isInline, bool isNormalized)
        {
            Font = font;
            Text = text;
            Color = color;
            IsInline = isInline;
            IsNormalized = isNormalized;
        }

        private static bool IsTextNormalized(bool isInline, string text)
        {
            return text.IndexOfAny(isInline ? InlineCharsToNormalize : CharsToBreakOn) < 0;
        }

        public IEnumerable<TextBlock> Normalize()
        {
            if (IsNormalized)
            {
                yield return this;
                yield break;
            }

            var text = NormalizeText();
            if (IsInline)
            {
                yield return CloneNormalized(text);
                yield break;
            }

            int start = 0;
            var length = text.Length;
            for (int i = 0; i < length; ++i)
            {
                if (text[i] == '\n')
                {
                    if (start != i) yield return CloneNormalized(text.Substring(start, i - start));
                    yield return CloneNormalized(LineBreak);
                    start = i + 1;
                }
                else if (char.IsWhiteSpace(text[i]))
                {
                    if (start != i) yield return CloneNormalized(text.Substring(start, i - start));
                    start = i + 1;
                }
            }
            if (start != length)
                yield return CloneNormalized(text.Substring(start, length - start));
        }

        private string NormalizeText()
        {
            var builder = new StringBuilder();

            int FindFirstNonSpace()
            {
                int pos = 0;
                while (pos < builder.Length && builder[pos] == ' ') ++pos;
                return pos;
            }

            int FindLastNonSpace()
            {
                int pos = builder.Length-1;
                while (pos >= 0&& builder[pos] == ' ') --pos;
                return pos;
            }

            var chars = IsInline ? InlineCharsToNormalize : CharsToNormalize;

            int start = 0;
            int index;
            while ((index = Text.IndexOfAny(chars, start)) >= 0)
            {
                if (start < index) builder.Append(Text, start, index - start);
                if (Text[index] != '\r' || (index + 1 < Text.Length && Text[index + 1] != '\n'))
                    builder.Append(' ');
                start = index + 1;
            }
            if (start < Text.Length) builder.Append(Text, start, Text.Length - start);

            int realStart = FindFirstNonSpace();
            int realEnd = FindLastNonSpace();

            return realStart > realEnd ? string.Empty : builder.ToString(realStart, realEnd - realStart + 1);
        }

        private TextBlock CloneNormalized(string text)
        {
            return new TextBlock(Font, text, Color, true, true);
        }
    }
}