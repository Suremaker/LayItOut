using System;
using System.Drawing;

namespace LayItOut.Components
{
    public class HBox : Container
    {
        public HorizontalAlignment ContentAlignment { get; set; }

        protected override Size OnMeasure(Size size)
        {
            var result = Size.Empty;

            foreach (var child in GetChildren())
            {
                child.Measure(size);
                var desiredSize = child.DesiredSize;

                result.Width += desiredSize.Width;
                result.Height = Math.Max(result.Height, desiredSize.Height);
                size.Width -= desiredSize.Width;
                if (size.Width < 0) size.Width = 0;
            }

            return result;
        }

        protected override void OnArrange()
        {
            var remainingSpace = MeasureRemainingSpace();
            var location = Layout.Location + remainingSpace.alignmentShift;
            var remainingSize = Layout.Size;
            var unlimitedIndex = 0;
            foreach (var child in GetChildren())
            {
                var childWidth = child.DesiredSize.Width;
                if (child.Width.IsUnlimited)
                    childWidth += remainingSpace.unlimitedBuffers[unlimitedIndex++];

                var childSize = new Size(Math.Min(childWidth, remainingSize.Width), remainingSize.Height);

                child.Arrange(new Rectangle(location, childSize));

                location.X += childSize.Width;
                remainingSize.Width -= childSize.Width;
            }
        }

        private (Size alignmentShift, int[] unlimitedBuffers) MeasureRemainingSpace()
        {
            var unlimited = 0;
            var remaining = Layout.Width;
            foreach (var child in GetChildren())
            {
                remaining -= child.DesiredSize.Width;
                if (child.Width.IsUnlimited)
                    ++unlimited;
            }

            if (unlimited > 0)
                return (Size.Empty, SizeUnit.Distribute(remaining, unlimited));
            return (ContentAlignment.GetShift(remaining), Array.Empty<int>());
        }
    }
}