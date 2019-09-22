using System.ComponentModel;

namespace LayItOut.Components
{
    [Description("Interface describing components containing a list of child-components.")]
    public interface IContainer : IComponent
    {
        void AddComponent(IComponent child);
    }
}