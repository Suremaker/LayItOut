using System.Drawing;

namespace LayItOut.Rendering
{
    public interface IRenderingContext
    {
        SizeF MeasureText(string text, Font font);
        float GetSpaceWidth(Font font);
    }
}
