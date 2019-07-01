using System.Drawing;

namespace LayItOut.Rendering
{
    public interface IRendererContext
    {
        SizeF MeasureText(string text, FontInfo font);
        float GetSpaceWidth(FontInfo font);
    }
}
