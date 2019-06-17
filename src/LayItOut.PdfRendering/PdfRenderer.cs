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
    public class PdfRenderer : Renderer<XGraphics>
    {
        public PdfRenderer()
        {
            RegisterRenderer(new PanelRenderer());
            RegisterRenderer(new TextRenderer<Link>());
            RegisterRenderer(new TextRenderer<Label>());
            RegisterRenderer(new TextRenderer<TextBox>());
            RegisterRenderer(new ImageRenderer());
        }

        public void Render(Form form, PdfPage pdfPage, PdfRendererOptions options = null)
        {
            options = options ?? PdfRendererOptions.Default;
            var size = options.AdjustPageSize
                ? new Size(int.MaxValue, int.MaxValue)
                : new Size(pdfPage.Width.ToLayout(), pdfPage.Height.ToLayout());

            using (var g = CreateGraphics(pdfPage, options))
            {
                form.LayOut(size, new RendererContext(g));

                if (options.AdjustPageSize)
                    AdjustPageSize(form, pdfPage, g);
            }
            using (var g = CreateGraphics(pdfPage, options))
                Render(g, form.Content);
        }

        private XGraphics CreateGraphics(PdfPage pdfPage, PdfRendererOptions options)
        {
            var g = XGraphics.FromPdfPage(pdfPage, XGraphicsUnit.Point);
            g.SmoothingMode = XSmoothingMode.HighQuality;
            options.ConfigureGraphics?.Invoke(g);
            return g;
        }

        private static void AdjustPageSize(Form form, PdfPage pdfPage, XGraphics g)
        {
            var points = new[]
            {
                new XPoint(0, 0),
                new XPoint(form.Content.Layout.Width, 0),
                new XPoint(form.Content.Layout.Width, form.Content.Layout.Height),
                new XPoint(0, form.Content.Layout.Height),
            };
            g.Transform.Transform(points);
            var width = Math.Max(0, points.Max(x => x.X));
            var height = Math.Max(0, points.Max(y => y.Y));
            pdfPage.Width = width;
            pdfPage.Height = height;
        }
    }
}
