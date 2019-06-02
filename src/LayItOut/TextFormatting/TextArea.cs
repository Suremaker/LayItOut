using System.Drawing;

namespace LayItOut.TextFormatting
{
    internal class TextArea : ITextArea
    {
        public TextArea(SizeF size, TextBlock block, float spacePadding)
        {
            Size = size;
            Block = block;
            SpacePadding = spacePadding;
        }

        public PointF Position { get; internal set; }
        public SizeF Size { get; }
        public TextBlock Block { get; }
        public RectangleF Area => new RectangleF(Position, Size);
        public float SpacePadding { get; }
    }
}