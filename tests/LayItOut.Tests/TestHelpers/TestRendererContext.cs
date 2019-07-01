using System.Drawing;
using LayItOut.Rendering;

namespace LayItOut.Tests.TestHelpers
{
    public class TestRendererContext : IRendererContext
    {
        public static readonly TestRendererContext Instance = new TestRendererContext();

        private TestRendererContext() { }

        public SizeF MeasureText(string text, FontInfo font)
        {
            return font.Size == 0
                ? Size.Empty
                : new SizeF(text.Length, GetHeight(font));
        }

        public float GetSpaceWidth(FontInfo font)
        {
            return 1;
        }

        /// <summary>
        /// Some simulated height based on font size - purely for testing purposes.
        /// </summary>
        public float GetHeight(FontInfo font)
        {
            return font.Size * 1.2f;
        }
    }
}
