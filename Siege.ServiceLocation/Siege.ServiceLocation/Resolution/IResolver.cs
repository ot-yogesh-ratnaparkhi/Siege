using System;

namespace Siege.ServiceLocation.Resolution
{
    public interface IResolver
    {
        object Resolve(Type type);
    }
}