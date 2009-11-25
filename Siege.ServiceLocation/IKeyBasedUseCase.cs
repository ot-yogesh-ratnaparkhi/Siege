namespace Siege.ServiceLocation
{
    public interface IKeyBasedUseCase : IUseCase
    {
        string Key { get; }
    }
    public interface IKeyBasedUseCase<TBaseService> : IKeyBasedUseCase, IUseCase<TBaseService> { }
}
