using System.Collections;

namespace Siege.ServiceLocation
{
    public interface IGenericFactory<TBaseType>
    {
        TBaseType Build(IDictionary constructorArguments);
    }
}
