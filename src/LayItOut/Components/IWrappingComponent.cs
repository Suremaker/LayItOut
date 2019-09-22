using System.ComponentModel;

namespace LayItOut.Components
{
    [Description("Interface describing a component containing child component.")]
    public interface IWrappingComponent : IComponent
    {
        IComponent Inner { set; }
    }
}