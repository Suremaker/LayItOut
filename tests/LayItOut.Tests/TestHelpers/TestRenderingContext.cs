using System.Drawing;
using LayItOut.Rendering;

namespace LayItOut.Tests.TestHelpers
{
    public class TestRenderingContext : IRenderingContext
    {
        public static readonly IRenderingContext Instance = new TestRenderingContext();

        private TestRenderingContext() { }

        public SizeF MeasureText(string text, Font font)
        {
            if (font.Size == 0)
                return Size.Empty;

            return new SizeF(text.Length, font.GetHeight());
        }

        public float GetSpaceWidth(Font font)
        {
            return 1;
        }
    }
}
