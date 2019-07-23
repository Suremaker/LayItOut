using System.Drawing;
using LayItOut.Components;
using LayItOut.Rendering;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering.Renderers
{
    internal class PanelRenderer : IComponentRenderer<PdfRendererContext, Panel>
    {
        public void Render(PdfRendererContext ctx, Panel panel)
        {
            if (panel.BackgroundColor.A == 0 && panel.Border.Color.A == 0)
                return;
            var brush = panel.BackgroundColor.A > 0 ? new XSolidBrush(panel.BackgroundColor.ToXColor()) : null;
            var pen = panel.Border.Size > 0 ? new XPen(panel.Border.Color.ToXColor(), panel.Border.Size) : null;

            var radius = panel.ActualRadius;
            var path = new XGraphicsPath { FillMode = XFillMode.Alternate };
            var shift = panel.Border.Size * 0.5f;
            var rect = new RectangleF(panel.BorderLayout.X + shift, panel.BorderLayout.Y + shift, panel.BorderLayout.Width - 2 * shift, panel.BorderLayout.Height - 2 * shift);

            path.AddLine(rect.X + radius.TopLeft, rect.Y, rect.Right - radius.TopRight, rect.Y);
            if (radius.TopRight > 0)
                path.AddArc(rect.Right - 2 * radius.TopRight, rect.Y, 2 * radius.TopRight, 2 * radius.TopRight, 270, 90);
            path.AddLine(rect.Right, rect.Y + radius.TopRight, rect.Right, rect.Bottom - radius.BottomRight);
            if (radius.BottomRight > 0)
                path.AddArc(rect.Right - 2 * radius.BottomRight, rect.Bottom - 2 * radius.BottomRight, 2 * radius.BottomRight, 2 * radius.BottomRight, 0, 90);
            path.AddLine(rect.Right - radius.BottomRight, rect.Bottom, rect.X + radius.BottomLeft, rect.Bottom);
            if (radius.BottomLeft > 0)
                path.AddArc(rect.X, rect.Bottom - 2 * radius.BottomLeft, 2 * radius.BottomLeft, 2 * radius.BottomLeft, 90, 90);
            path.AddLine(rect.Left, rect.Bottom - radius.BottomLeft, rect.Left, rect.Top + radius.TopLeft);
            if (radius.TopLeft > 0)
                path.AddArc(rect.Left, rect.Top, 2 * radius.TopLeft, 2 * radius.TopLeft, 180, 90);
            path.CloseFigure();

            if (pen != null && brush != null)
                ctx.Graphics.DrawPath(pen, brush, path);
            else if (pen != null)
                ctx.Graphics.DrawPath(pen, path);
            else
                ctx.Graphics.DrawPath(brush, path);
        }
    }
}