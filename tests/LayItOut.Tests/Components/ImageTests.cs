﻿using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using LayItOut.Attributes;
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
            img.Measure(new Size(100, 100), TestRendererContext.Instance);
            img.DesiredSize.ShouldBe(Size.Empty);
        }

        [Fact]
        public void Measure_should_use_image_dimensions()
        {
            var img = new Image { Src = new Bitmap(10, 20) };
            img.Measure(new Size(100, 100), TestRendererContext.Instance);
            img.DesiredSize.ShouldBe(new Size(10, 20));
        }

        [Theory]
        [InlineData("fill", "10x200", "5:5|100x30", "top left", "0:0|10x200", "5:5|100x30")]
        [InlineData("none", "20x30", "5:5|30x20", "top left", "0:0|20x20", "5:5|20x20")]
        [InlineData("none", "20x30", "5:5|30x20", "bottom right", "0:10|20x20", "15:5|20x20")]
        [InlineData("none", "21x30", "5:5|30x21", "center", "0:4.5|21x21", "9.5:5|21x21")]
        [InlineData("uniform", "15x30", "5:5|50x45", "center", "0:0|15x30", "18.75:5|22.5x45")]
        [InlineData("uniform", "15x30", "5:5|50x45", "top left", "0:0|15x30", "5:5|22.5x45")]
        [InlineData("uniform", "15x30", "5:5|50x45", "top right", "0:0|15x30", "32.5:5|22.5x45")]
        [InlineData("uniform", "15x30", "5:5|10x50", "center", "0:0|15x30", "5:20|10x20")]
        [InlineData("uniform", "15x30", "5:5|10x50", "top left", "0:0|15x30", "5:5|10x20")]
        [InlineData("uniform", "15x30", "5:5|10x50", "bottom left", "0:0|15x30", "5:35|10x20")]
        public void Arrange_should_arrange_image_accordingly_to_scaling_and_aligning_options(string scaling, string image, string area, string alignment, string expectedSourceRegion, string expectedImageLayout)
        {
            var imgSize = ToSize(image);
            var areaRect = ToRect(area);
            var img = new Image
            {
                Src = new Bitmap(imgSize.Width, imgSize.Height),
                Scaling = Enum.Parse<ImageScaling>(scaling, true),
                Alignment = Alignment.Parse(alignment),
                Width = SizeUnit.Unlimited,
                Height = SizeUnit.Unlimited
            };
            img.Measure(areaRect.Size, TestRendererContext.Instance);
            img.Arrange(areaRect);

            img.ImageSourceRegion.ShouldBe(ToRectF(expectedSourceRegion));
            img.ImageLayout.ShouldBe(ToRectF(expectedImageLayout));
        }

        private Size ToSize(string text)
        {
            var parts = text.Split('x').Select(int.Parse).ToArray();
            return new Size(parts[0], parts[1]);
        }

        private SizeF ToSizeF(string text)
        {
            var parts = text.Split('x').Select(s => float.Parse(s, CultureInfo.InvariantCulture)).ToArray();
            return new SizeF(parts[0], parts[1]);
        }

        private Point ToPoint(string text)
        {
            var parts = text.Split(':').Select(int.Parse).ToArray();
            return new Point(parts[0], parts[1]);
        }

        private PointF ToPointF(string text)
        {
            var parts = text.Split(':').Select(s => float.Parse(s, CultureInfo.InvariantCulture)).ToArray();
            return new PointF(parts[0], parts[1]);
        }

        private Rectangle ToRect(string text)
        {
            var parts = text.Split('|');
            return new Rectangle(ToPoint(parts[0]), ToSize(parts[1]));
        }

        private RectangleF ToRectF(string text)
        {
            var parts = text.Split('|');
            return new RectangleF(ToPointF(parts[0]), ToSizeF(parts[1]));
        }
    }
}
