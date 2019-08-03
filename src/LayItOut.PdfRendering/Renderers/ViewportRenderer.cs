using System;
using System.Drawing;
using LayItOut.Components;
using LayItOut.Rendering;
using PdfSharp.Drawing;

namespace LayItOut.PdfRendering.Renderers
{
    internal class ViewportRenderer : ComponentRenderer<PdfRendererContext, Viewport>
    {
        public override void Render(PdfRendererContext ctx, Viewport viewport, Action<PdfRendererContext, IComponent> renderChild)
        {
            if (viewport.Inner == null)
                return;
            var state = ctx.Graphics.Save();
            try
            {
                ctx.Graphics.IntersectClip(viewport.ActualViewRegion.ToXRect());
                foreach (var child in viewport.GetChildren())
                    renderChild(ctx, child);
            }
            finally
            {
                ctx.Graphics.Restore(state);
            }
        }
    }
}