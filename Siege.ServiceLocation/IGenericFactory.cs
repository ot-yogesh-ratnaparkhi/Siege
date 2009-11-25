using System.Collections;

namespace Siege.ServiceLocation
{
    public interface IGenericFactory<TBaseService>
    {
        TBaseService Build(IDictionary constructorArguments);
    }
}
