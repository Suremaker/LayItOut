using System;
using LayItOut.Components;

namespace LayItOut.Rendering
{
    public class ComponentRenderer<TRendererContext, TComponent> : IComponentRenderer<TRendererContext, TComponent> where TComponent : IComponent
    {
        public virtual void Render(TRendererContext context, TComponent component, Action<TRendererContext, IComponent> renderChild)
        {
            OnRender(context, component);
            foreach (var child in component.GetChildren())
                renderChild(context, child);
        }

        protected virtual void OnRender(TRendererContext graphics, TComponent component) { }
    }
}