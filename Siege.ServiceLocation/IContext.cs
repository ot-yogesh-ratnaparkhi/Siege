namespace Siege.ServiceLocation
{
    public interface IContext
    {
        object GetValue();
    }

    public interface IContext<T> : IContext
    {
        T Value { get; }
    }
}
