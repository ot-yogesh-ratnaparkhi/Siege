namespace Siege.ServiceLocation
{
    public interface IGenericFactory<TBaseService>
    {
        TBaseService Build();
    }
}
