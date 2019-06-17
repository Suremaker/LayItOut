using System.Drawing;
using LayItOut.Components;
using LayItOut.Rendering;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering.Renderers
{
    internal class PanelRenderer : IComponentRenderer<XGraphics, Panel>
    {
        public void Render(XGraphics graphics, Panel panel)
        {
            DrawBackground(graphics, panel);
            DrawHorizontalBorder(graphics, panel, panel.BorderLayout.Top, panel.ActualBorder.Top, panel.Border.Top.Color);
            DrawHorizontalBorder(graphics, panel, panel.PaddingLayout.Bottom, panel.ActualBorder.Bottom, panel.Border.Bottom.Color);
            DrawVerticalBorder(graphics, panel, panel.BorderLayout.Left, panel.ActualBorder.Left, panel.Border.Left.Color);
            DrawVerticalBorder(graphics, panel, panel.PaddingLayout.Right, panel.ActualBorder.Right, panel.Border.Right.Color);
        }

        private void DrawHorizontalBorder(XGraphics graphics, Panel panel, int y, SizeUnit borderSize, Color borderColor)
        {
            if (!borderSize.IsAbsolute || borderColor.A == 0)
                return;
            var yCenter = y + borderSize.Value * 0.5f;
            var pen = new XPen(borderColor.ToXColor(), borderSize.ToXWidth());
            graphics.DrawLine(pen, panel.BorderLayout.Left, yCenter, panel.BorderLayout.Right, yCenter);
        }

        private void DrawVerticalBorder(XGraphics graphics, Panel panel, int x, SizeUnit borderSize, Color borderColor)
        {
            if (!borderSize.IsAbsolute || borderColor.A == 0)
                return;
            var xCenter = x + borderSize.Value * 0.5f;
            var pen = new XPen(borderColor.ToXColor(), borderSize.ToXWidth());
            graphics.DrawLine(pen, xCenter, panel.BorderLayout.Top, xCenter, panel.BorderLayout.Bottom);
        }

        private void DrawBackground(XGraphics graphics, Panel panel)
        {
            if (panel.BackgroundColor.A == 0)
                return;
            var brush = new XSolidBrush(panel.BackgroundColor.ToXColor());

            if (panel.BorderRadius.Equals(BorderRadius.None))
                graphics.DrawRectangle(brush, panel.PaddingLayout.ToXRect());
            else
            {
                var rect = panel.PaddingLayout;
                var radius = panel.ActualRadius;
                var path = new XGraphicsPath { FillMode = XFillMode.Alternate };

                path.AddLine(rect.X + radius.TopLeft, rect.Y, rect.Right - radius.TopRight, rect.Y);
                if (radius.TopRight > 0)
                    path.AddArc(rect.Right - radius.TopRight, rect.Y, radius.TopRight, radius.TopRight, 270, 90);
                path.AddLine(rect.Right, rect.Y + radius.TopRight, rect.Right, rect.Bottom - radius.BottomRight);
                if (radius.BottomRight > 0)
                    path.AddArc(rect.Right-radius.BottomRight, rect.Bottom - radius.BottomRight, radius.BottomRight, radius.BottomRight, 0, 90);
                path.AddLine(rect.Right - radius.BottomRight, rect.Bottom, rect.X + radius.BottomLeft, rect.Bottom);
                if (radius.BottomLeft > 0)
                    path.AddArc(rect.X, rect.Bottom-radius.BottomLeft, radius.BottomLeft, radius.BottomLeft, 90, 90);
                path.AddLine(rect.Left, rect.Bottom - radius.BottomLeft, rect.Left, rect.Top + radius.TopLeft);
                if (radius.TopLeft > 0)
                    path.AddArc(rect.Left, rect.Top, radius.TopLeft, radius.TopLeft, 180, 90);
                path.CloseFigure();
                graphics.DrawPath(brush, path);
            }
        }
    }
}