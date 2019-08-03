using System;
using LayItOut.Components;
using LayItOut.Rendering;

namespace LayItOut.BitmapRendering.Renderers
{
    internal class ViewportRenderer : ComponentRenderer<BitmapRendererContext, Viewport>
    {
        public override void Render(BitmapRendererContext ctx, Viewport viewport, Action<BitmapRendererContext, IComponent> renderChild)
        {
            if (viewport.Inner == null)
                return;
            var state = ctx.Graphics.Save();
            try
            {
                ctx.Graphics.IntersectClip(viewport.ActualViewRegion);
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