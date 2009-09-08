namespace Siege.ServiceLocation
{
    public interface IDefaultUseCase : IUseCase {}
    public interface IDefaultUseCase<TBaseType> : IDefaultUseCase, IUseCase<TBaseType> {}
}