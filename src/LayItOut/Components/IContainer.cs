namespace LayItOut.Components
{
    public interface IContainer : IComponent
    {
        void AddComponent(IComponent child);
    }
}