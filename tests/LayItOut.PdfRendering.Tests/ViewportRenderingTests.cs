using System.Drawing;
using LayItOut.Attributes;
using LayItOut.Components;
using LayItOut.PdfRendering.Tests.Helpers;
using PdfSharp.Pdf;
using Xunit;

namespace LayItOut.PdfRendering.Tests
{
    public class ViewportRenderingTests : PdfTests
    {
        [Fact]
        public void It_should_render_viewports()
        {
            var renderer = new PdfRenderer();

            var stack = new Stack();
            stack.AddComponent(new Panel
            {
                Width = 100,
                Height = 100,
                Border = Border.Parse("1 #808080"),
                BackgroundColor = Color.LightBlue,
                BorderRadius = BorderRadius.Parse("25")
            });
            stack.AddComponent(new Viewport
            {
                Alignment = Alignment.Parse("center left"),
                Width = 10,
                Inner = new Panel
                {
                    Width = 40,
                    Height = 40,
                    Border = Border.Parse("1 #808080"),
                    BackgroundColor = Color.White,
                    BorderRadius = BorderRadius.Parse("20")
                },
                ContentAlignment = Alignment.Parse("center right")
            });
            stack.AddComponent(new Viewport
            {
                Alignment = Alignment.Parse("top center"),
                Height = 10,
                Inner = new Panel
                {
                    Width = 40,
                    Height = 40,
                    Border = Border.Parse("1 #808080"),
                    BackgroundColor = Color.White,
                    BorderRadius = BorderRadius.Parse("20")
                },
                ContentAlignment = Alignment.Parse("bottom center")
            });
            stack.AddComponent(new Viewport
            {
                Alignment = Alignment.Parse("center right"),
                Width = 10,
                Inner = new Panel
                {
                    Width = 40,
                    Height = 40,
                    Border = Border.Parse("1 #808080"),
                    BackgroundColor = Color.White,
                    BorderRadius = BorderRadius.Parse("20")
                },
                ContentAlignment = Alignment.Parse("center left")
            });
            stack.AddComponent(new Viewport
            {
                Alignment = Alignment.Parse("bottom center"),
                Height = 10,
                Inner = new Panel
                {
                    Width = 40,
                    Height = 40,
                    Border = Border.Parse("1 #808080"),
                    BackgroundColor = Color.White,
                    BorderRadius = BorderRadius.Parse("20")
                },
                ContentAlignment = Alignment.Parse("top center")
            });
            stack.AddComponent(new Viewport
            {
                Alignment = Alignment.Parse("center center"),
                ClipMargin = Spacer.Parse("3 1"),
                Inner = new Panel
                {
                    Width = 40,
                    Height = 40,
                    BackgroundColor = Color.White,
                    Border = Border.Parse("1 #808080"),
                    BorderRadius = BorderRadius.Parse("20")
                },
                ContentAlignment = Alignment.Parse("center center")
            });
            var form = new Form(stack);

            var doc = new PdfDocument();
            renderer.Render(form, doc.AddPage(), new PdfRendererOptions { AdjustPageSize = true });
            PdfImageComparer.ComparePdfs("viewport", doc);
        }

        [Fact]
        public void It_should_render_viewports_with_margin()
        {
            var renderer = new PdfRenderer();

            var stack = new Stack();
            stack.AddComponent(new Panel
            {
                Width = 100,
                Height = 100,
                Border = Border.Parse("1 #808080"),
                BackgroundColor = Color.LightBlue,
                BorderRadius = BorderRadius.Parse("25")
            });
            stack.AddComponent(new Viewport
            {
                Alignment = Alignment.Parse("top left"),
                Width = 10,
                Height = 10,
                ClipMargin = Spacer.Parse("3"),
                Inner = new Panel
                {
                    Width = 40,
                    Height = 40,
                    Border = Border.Parse("1 #808080"),
                    BackgroundColor = Color.LightBlue,
                    BorderRadius = BorderRadius.Parse("20")
                },
                ContentAlignment = Alignment.Parse("bottom right")
            });
            stack.AddComponent(new Viewport
            {
                Alignment = Alignment.Parse("top right"),
                Width = 10,
                Height = 10,
                ClipMargin = Spacer.Parse("3"),
                Inner = new Panel
                {
                    Width = 40,
                    Height = 40,
                    Border = Border.Parse("1 #808080"),
                    BackgroundColor = Color.LightBlue,
                    BorderRadius = BorderRadius.Parse("20")
                },
                ContentAlignment = Alignment.Parse("bottom left")
            });
            stack.AddComponent(new Viewport
            {
                Alignment = Alignment.Parse("bottom left"),
                Width = 10,
                Height = 10,
                ClipMargin = Spacer.Parse("3"),
                Inner = new Panel
                {
                    Width = 40,
                    Height = 40,
                    Border = Border.Parse("1 #808080"),
                    BackgroundColor = Color.LightBlue,
                    BorderRadius = BorderRadius.Parse("20")
                },
                ContentAlignment = Alignment.Parse("top right")
            });
            stack.AddComponent(new Viewport
            {
                Alignment = Alignment.Parse("bottom right"),
                Width = 10,
                Height = 10,
                ClipMargin = Spacer.Parse("3"),
                Inner = new Panel
                {
                    Width = 40,
                    Height = 40,
                    Border = Border.Parse("1 #808080"),
                    BackgroundColor = Color.LightBlue,
                    BorderRadius = BorderRadius.Parse("20")
                },
                ContentAlignment = Alignment.Parse("top left")
            });

            var form = new Form(stack);

            var doc = new PdfDocument();
            renderer.Render(form, doc.AddPage(), new PdfRendererOptions { AdjustPageSize = true });
            PdfImageComparer.ComparePdfs("viewport2", doc);
        }

    }
}