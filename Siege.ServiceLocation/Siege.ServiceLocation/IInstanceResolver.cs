using System;

namespace Siege.ServiceLocation
{
    public interface IInstanceResolver
    {
        object GetInstance(Type type, string key);
        object GetInstance(Type type);
    }
}