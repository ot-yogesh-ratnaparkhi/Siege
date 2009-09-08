using System.Collections.Generic;

namespace Siege.Container
{
    public interface IContextStore
    {
        void Add(object contextItem);
        List<object> Items { get; }
    }
}
