namespace LayItOut.Components
{
    public interface IWrappingComponent : IComponent
    {
        IComponent Inner { get; set; }
    }
}