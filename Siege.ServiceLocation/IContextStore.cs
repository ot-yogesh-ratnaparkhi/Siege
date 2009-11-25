using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IContextStore
    {
        void Add(object contextItem);
        List<object> Items { get; }
    }
}