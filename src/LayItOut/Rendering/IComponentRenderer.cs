using System;
using LayItOut.Components;

namespace LayItOut.Rendering
{
    public interface IComponentRenderer<TRendererContext, in TComponent> where TComponent : IComponent
    {
        void Render(TRendererContext context, TComponent component, Action<TRendererContext, IComponent> renderChild);
    }
}
