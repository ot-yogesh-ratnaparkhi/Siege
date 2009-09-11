namespace Siege.ServiceLocation
{
    public interface IKeyBasedUseCase : IUseCase
    {
        string Key { get; }
    }
    public interface IKeyBasedUseCase<TBaseType> : IKeyBasedUseCase, IUseCase<TBaseType> { }
}
