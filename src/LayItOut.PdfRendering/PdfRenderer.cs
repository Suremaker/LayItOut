using System;
using System.Drawing;
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
            RegisterRenderer(new LabelRenderer());
            RegisterRenderer(new LinkRenderer());
            RegisterRenderer(new ImageRenderer());
            _configureGraphics = configureGraphics;
            _adjustPageSize = adjustPageSize;
        }

        public void Render(Form form, PdfPage pdfPage)
        {
            var size = _adjustPageSize
                ? new Size(int.MaxValue, int.MaxValue)
                : new Size(pdfPage.Width.ToLayout(), pdfPage.Height.ToLayout());

            using (var g = XGraphics.FromPdfPage(pdfPage, XGraphicsUnit.Point))
            {
                ConfigureGraphics(g);
                form.LayOut(size, new RendererContext(g));
                if (_adjustPageSize)
                {
                    pdfPage.Width = form.Content.Layout.Width;
                    pdfPage.Height = form.Content.Layout.Height;
                }

                Render(g, form.Content);
            }
        }

        private void ConfigureGraphics(XGraphics g)
        {
            g.SmoothingMode = XSmoothingMode.HighQuality;
            _configureGraphics?.Invoke(g);
        }
    }
}
