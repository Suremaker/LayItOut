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

        public PdfRenderer(Action<XGraphics> configureGraphics = null)
        {
            RegisterRenderer(new PanelRenderer());
            _configureGraphics = configureGraphics;
        }

        public void Render(Form form, PdfPage pdfPage)
        {
            form.LayOut(new Size(pdfPage.Width.ToLayout(), pdfPage.Height.ToLayout()));

            using (var g = XGraphics.FromPdfPage(pdfPage, XGraphicsUnit.Point))
            {
                ConfigureGraphics(g);
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
