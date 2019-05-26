using System;
using System.Drawing;

namespace LayItOut.Components
{
    public class VBox : Container
    {
        public VerticalAlignment ContentAlignment { get; set; }

        protected override Size OnMeasure(Size size)
        {
            var result = Size.Empty;

            foreach (var child in GetChildren())
            {
                child.Measure(size);
                var desiredSize = child.DesiredSize;

                result.Height += desiredSize.Height;
                result.Width = Math.Max(result.Width, desiredSize.Width);
                size.Height -= desiredSize.Height;
                if (size.Height < 0) size.Height = 0;
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
                var childHeight = child.DesiredSize.Height;
                if (child.Height.IsUnlimited)
                    childHeight += remainingSpace.unlimitedBuffers[unlimitedIndex++];

                var childSize = new Size(remainingSize.Width, Math.Min(childHeight, remainingSize.Height));

                child.Arrange(new Rectangle(location, childSize));

                location.Y += childSize.Height;
                remainingSize.Height -= childSize.Height;
            }
        }

        private (Size alignmentShift, int[] unlimitedBuffers) MeasureRemainingSpace()
        {
            var unlimited = 0;
            var remaining = Layout.Height;
            foreach (var child in GetChildren())
            {
                remaining -= child.DesiredSize.Height;
                if (child.Height.IsUnlimited)
                    ++unlimited;
            }

            if (unlimited > 0)
                return (Size.Empty, SizeUnit.Distribute(remaining, unlimited));
            return (ContentAlignment.GetShift(remaining), Array.Empty<int>());
        }
    }
}