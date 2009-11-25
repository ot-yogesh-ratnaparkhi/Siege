namespace Siege.ServiceLocation
{
    public interface IDefaultUseCase : IUseCase {}
    public interface IDefaultUseCase<TBaseService> : IDefaultUseCase, IUseCase<TBaseService> {}
}