using System.Drawing;

namespace LayItOut.TextFormatting
{
    public interface ITextArea
    {
        PointF Position { get; }
        SizeF Size { get; }
        RectangleF Area { get; }
        float SpacePadding { get; }
        TextBlock Block { get; }
    }
}