using System;
using System.Collections.Generic;
using LayItOut.Components;

namespace LayItOut.Rendering
{
    public abstract class Renderer<TRendererContext>
    {
        private readonly Dictionary<Type, Action<TRendererContext, IComponent>> _renderers = new Dictionary<Type, Action<TRendererContext, IComponent>>();

        public void RegisterRenderer<TComponent>(IComponentRenderer<TRendererContext, TComponent> renderer) where TComponent : IComponent
        {
            _renderers[typeof(TComponent)] = (ctx, c) => renderer.Render(ctx, (TComponent)c);
        }

        protected void Render(TRendererContext ctx, IComponent component)
        {
            if (component.Layout.Size.Width * component.Layout.Size.Height == 0)
                return;

            if (_renderers.TryGetValue(component.GetType(), out var render))
                render(ctx, component);
            foreach (var child in component.GetChildren())
                Render(ctx, child);
        }
    }
}