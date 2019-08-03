using System;
using System.Drawing;
using System.Linq;
using LayItOut.Components;
using LayItOut.PdfRendering.Renderers;
using LayItOut.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace LayItOut.PdfRendering
{
    public class PdfRenderer : Renderer<PdfRendererContext>
    {
        public PdfFontResolver FontResolver { get; }

        public PdfRenderer(PdfFontResolver fontResolver = null)
        {
            FontResolver = fontResolver ?? new PdfFontResolver();
            RegisterRenderer(new PanelRenderer());
            RegisterRenderer(new TextRenderer<Link>());
            RegisterRenderer(new TextRenderer<Label>());
            RegisterRenderer(new TextRenderer<TextBox>());
            RegisterRenderer(new ImageRenderer());
        }

        public void Render(Form form, PdfPage pdfPage, PdfRendererOptions options = null)
        {
            options = options ?? PdfRendererOptions.Default;


            using (var g = CreateGraphics(pdfPage, options))
            {
                var size = DetermineMaxSize(pdfPage, options, g);
                form.LayOut(size, CreateContext(g));

                if (options.AdjustPageSize)
                    AdjustPageSize(form, pdfPage, g);
            }
            using (var g = CreateGraphics(pdfPage, options))
                Render(CreateContext(g), form.Content);
        }

        private static Size DetermineMaxSize(PdfPage pdfPage, PdfRendererOptions options, XGraphics g)
        {
            if (options.AdjustPageSize)
                return new Size(int.MaxValue, int.MaxValue);

            var pageSize = new Size(pdfPage.Width.ToLayout(), pdfPage.Height.ToLayout());
            var scaledSize = GetTransformedSize(g, pageSize);
            var upscaledSize = new Size(
                (int)(pageSize.Width / scaledSize.Width * pageSize.Width),
                (int)(pageSize.Height / scaledSize.Height * pageSize.Height));
            return upscaledSize;
        }

        private PdfRendererContext CreateContext(XGraphics g) => new PdfRendererContext(g, FontResolver);

        private XGraphics CreateGraphics(PdfPage pdfPage, PdfRendererOptions options)
        {
            var g = XGraphics.FromPdfPage(pdfPage, XGraphicsUnit.Point);
            g.SmoothingMode = XSmoothingMode.HighQuality;
            options.ConfigureGraphics?.Invoke(g);
            return g;
        }

        private static void AdjustPageSize(Form form, PdfPage pdfPage, XGraphics g)
        {
            var size = GetTransformedSize(g, form.Content.Layout.Size);
            pdfPage.Width = size.Width;
            pdfPage.Height = size.Height;
        }

        private static SizeF GetTransformedSize(XGraphics g, Size size)
        {
            var points = new[]
            {
                new XPoint(0, 0),
                new XPoint(size.Width, 0),
                new XPoint(size.Width, size.Height),
                new XPoint(0, size.Height),
            };
            g.Transform.Transform(points);
            var width = Math.Max(0, points.Max(x => x.X) - points.Min(x => x.X));
            var height = Math.Max(0, points.Max(y => y.Y) - points.Min(y => y.Y));
            return new SizeF((float)width, (float)height);
        }
    }
}
