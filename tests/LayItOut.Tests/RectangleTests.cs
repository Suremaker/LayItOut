using System.Drawing;
using LayItOut.Attributes;
using Shouldly;
using Xunit;

namespace LayItOut.Tests
{
    public class RectangleTests
    {
        [Fact]
        public void ShrinkBy_should_shrink_rectangle()
        {
            var rect = new Rectangle(5, 10, 20, 40);
            rect.ShrinkBy(new Spacer(1, 2, 3, 4)).ShouldBe(new Rectangle(7, 11, 14, 36));
        }

        [Fact]
        public void ShrinkBy_should_never_get_below_0_and_location_not_outside_of_bounding_box()
        {
            new Rectangle(5, 10, 5, 45)
                .ShrinkBy(new Spacer(10, 20, 30, 40))
                .ShouldBe(new Rectangle(new Point(10, 20), Size.Empty));

            new Rectangle(5, 10, 45, 5)
                .ShrinkBy(new Spacer(10, 20, 30, 40))
                .ShouldBe(new Rectangle(new Point(25, 15), Size.Empty));
        }
    }
}
