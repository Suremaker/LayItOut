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
        private readonly Action<XGraphics> _configureGraphics;
        private readonly bool _adjustPageSize;

        public PdfRenderer(Action<XGraphics> configureGraphics = null, bool adjustPageSize = false)
        {
            RegisterRenderer(new PanelRenderer());
            RegisterRenderer(new TextRenderer<Link>());
            RegisterRenderer(new TextRenderer<Label>());
            RegisterRenderer(new TextRenderer<TextBox>());
            RegisterRenderer(new ImageRenderer());
            _configureGraphics = configureGraphics;
            _adjustPageSize = adjustPageSize;
        }

        public void Render(Form form, PdfPage pdfPage)
        {
            var size = _adjustPageSize
                ? new Size(int.MaxValue, int.MaxValue)
                : new Size(pdfPage.Width.ToLayout(), pdfPage.Height.ToLayout());

            using (var g = CreateGraphics(pdfPage))
            {
                form.LayOut(size, new RendererContext(g));

                if (_adjustPageSize)
                    AdjustPageSize(form, pdfPage, g);
            }
            using (var g = CreateGraphics(pdfPage))
                Render(g, form.Content);
        }

        private XGraphics CreateGraphics(PdfPage pdfPage)
        {
            var g = XGraphics.FromPdfPage(pdfPage, XGraphicsUnit.Point);
            ConfigureGraphics(g);
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

        private void ConfigureGraphics(XGraphics g)
        {
            g.SmoothingMode = XSmoothingMode.HighQuality;
            _configureGraphics?.Invoke(g);
        }
    }
}
