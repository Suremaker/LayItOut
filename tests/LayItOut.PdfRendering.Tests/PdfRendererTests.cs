using System.Drawing;
using LayItOut.Attributes;
using LayItOut.Components;
using PdfSharp.Pdf;
using Shouldly;
using Xunit;

namespace LayItOut.PdfRendering.Tests
{
    public class PdfRendererTests
    {
        [Fact]
        public void Render_should_use_page_size()
        {
            var renderer = new PdfRenderer();
            var form = new Form(new Panel { Width = SizeUnit.Unlimited, Height = SizeUnit.Unlimited });
            var page = new PdfDocument().AddPage();
            page.Width = 600;
            page.Height = 400;
            renderer.Render(form, page);

            form.Content.Layout.ShouldBe(new Rectangle(0, 0, 600, 400));

            page.Width.ShouldBe(600);
            page.Height.ShouldBe(400);
        }

        [Fact]
        public void Render_should_adjust_page_size()
        {
            var renderer = new PdfRenderer();
            var form = new Form(new Panel { Width = 200, Height = 100 });
            var page = new PdfDocument().AddPage();
            page.Width = 600;
            page.Height = 400;
            renderer.Render(form, page, new PdfRendererOptions { AdjustPageSize = true });

            page.Width.ShouldBe(200);
            page.Height.ShouldBe(100);
        }

        [Fact]
        public void Render_should_adjust_page_size_honoring_transformation()
        {
            var renderer = new PdfRenderer();
            var form = new Form(new Panel { Width = 200, Height = 100 });
            var page = new PdfDocument().AddPage();
            page.Width = 600;
            page.Height = 400;
            renderer.Render(form, page, new PdfRendererOptions
            {
                AdjustPageSize = true,
                ConfigureGraphics = g => g.ScaleTransform(0.5, 0.5)
            });

            page.Width.ShouldBe(100);
            page.Height.ShouldBe(50);
        }
    }
}
