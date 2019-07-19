using System.Drawing;
using LayItOut.Attributes;

namespace LayItOut.Rendering
{
    public interface IRendererContext
    {
        SizeF MeasureText(string text, FontInfo font);
        float GetSpaceWidth(FontInfo font);
    }
}
