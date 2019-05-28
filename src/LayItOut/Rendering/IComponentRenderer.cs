using LayItOut.Components;

namespace LayItOut.Rendering
{
    public interface IComponentRenderer<in TGraphics, in TComponent> where TComponent : IComponent
    {
        void Render(TGraphics graphics, TComponent component);
    }
}
