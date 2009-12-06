using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public class GlobalContextStore : IContextStore
    {
        private readonly List<object> context = new List<object>();

        public void Add(object contextItem)
        {
            context.Add(contextItem);
        }

        public List<object> Items
        {
            get { return context; }
        }
    }
}