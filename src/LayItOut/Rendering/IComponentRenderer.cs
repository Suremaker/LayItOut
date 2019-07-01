using LayItOut.Components;

namespace LayItOut.Rendering
{
    public interface IComponentRenderer<in TRendererContext, in TComponent> where TComponent : IComponent
    {
        void Render(TRendererContext graphics, TComponent component);
    }
}
