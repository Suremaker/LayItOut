using System;
using System.Collections.Generic;
using LayItOut.Components;

namespace LayItOut.Rendering
{
    public abstract class Renderer<TGraphics>
    {
        private readonly Dictionary<Type, Action<TGraphics, IComponent>> _renderers = new Dictionary<Type, Action<TGraphics, IComponent>>();

        public void RegisterRenderer<TComponent>(IComponentRenderer<TGraphics, TComponent> renderer) where TComponent : IComponent
        {
            _renderers[typeof(TComponent)] = (g, c) => renderer.Render(g, (TComponent)c);
        }

        protected void Render(TGraphics g, IComponent component)
        {
            if (component.Layout.Size.Width * component.Layout.Size.Height == 0)
                return;

            if (_renderers.TryGetValue(component.GetType(), out var render))
                render(g, component);
            foreach (var child in component.GetChildren())
                Render(g, child);
        }
    }
}