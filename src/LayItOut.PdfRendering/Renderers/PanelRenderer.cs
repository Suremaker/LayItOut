using System.Collections.Generic;
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
            graphics.DrawRectangle(brush, panel.PaddingLayout.ToXRect());
        }
    }
}