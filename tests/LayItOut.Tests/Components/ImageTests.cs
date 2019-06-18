using System.Drawing;
using LayItOut.Tests.TestHelpers;
using Shouldly;
using Xunit;
using Image = LayItOut.Components.Image;

namespace LayItOut.Tests.Components
{
    public class ImageTests
    {
        [Fact]
        public void Measure_should_handle_empty_images()
        {
            var img = new Image();
            img.Measure(new Size(100, 100), TestRenderingContext.Instance);
            img.DesiredSize.ShouldBe(Size.Empty);
        }

        [Fact]
        public void Measure_should_use_image_dimensions()
        {
            var img = new Image { Src = new Bitmap(10, 20) };
            img.Measure(new Size(100, 100), TestRenderingContext.Instance);
            img.DesiredSize.ShouldBe(new Size(10, 20));
        }
    }
}
