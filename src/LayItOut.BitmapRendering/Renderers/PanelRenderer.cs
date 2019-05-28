﻿using System.Drawing;
using System.Drawing.Drawing2D;
using LayItOut.Components;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering.Renderers
{
    internal class PanelRenderer : IComponentRenderer<Graphics, Panel>
    {
        public void Render(Graphics graphics, Panel panel)
        {
            DrawBackground(graphics, panel);
            DrawHorizontalBorder(graphics, panel, panel.BorderLayout.Top, panel.ActualBorder.Top, panel.Border.Top.Color);
            DrawHorizontalBorder(graphics, panel, panel.PaddingLayout.Bottom, panel.ActualBorder.Bottom, panel.Border.Bottom.Color);
            DrawVerticalBorder(graphics, panel, panel.BorderLayout.Left, panel.ActualBorder.Left, panel.Border.Left.Color);
            DrawVerticalBorder(graphics, panel, panel.PaddingLayout.Right, panel.ActualBorder.Right, panel.Border.Right.Color);
        }

        private void DrawHorizontalBorder(Graphics graphics, Panel panel, int y, SizeUnit borderSize, Color borderColor)
        {
            if (!borderSize.IsAbsolute || borderColor.A == 0)
                return;
            var yCenter = y + borderSize.Value * 0.5f;
            using (var pen = new Pen(borderColor, borderSize.Value){EndCap = LineCap.NoAnchor, StartCap = LineCap.NoAnchor})
                graphics.DrawLine(pen, panel.BorderLayout.Left, yCenter, panel.BorderLayout.Right, yCenter);
        }

        private void DrawVerticalBorder(Graphics graphics, Panel panel, int x, SizeUnit borderSize, Color borderColor)
        {
            if (!borderSize.IsAbsolute || borderColor.A == 0)
                return;
            var xCenter = x + borderSize.Value * 0.5f;
            using (var pen = new Pen(borderColor, borderSize.Value){EndCap = LineCap.NoAnchor, StartCap = LineCap.NoAnchor})
                graphics.DrawLine(pen, xCenter, panel.BorderLayout.Top, xCenter, panel.BorderLayout.Bottom);
        }

        private void DrawBackground(Graphics graphics, Panel panel)
        {
            if (panel.BackgroundColor.A == 0)
                return;
            using (var brush = new SolidBrush(panel.BackgroundColor))
                graphics.FillRectangle(brush, panel.PaddingLayout);
        }
    }
}