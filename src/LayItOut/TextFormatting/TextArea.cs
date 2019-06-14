using System.Drawing;

namespace LayItOut.TextFormatting
{
    public class TextArea
    {
        public TextArea(PointF position, SizeF size, TextBlock block, float spacePadding)
        {
            Position = position;
            Size = size;
            Block = block;
            SpacePadding = spacePadding;
        }

        public PointF Position { get; }
        public SizeF Size { get; }
        public TextBlock Block { get; }
        public RectangleF Area => new RectangleF(Position, Size);
        public float SpacePadding { get; }
    }
}