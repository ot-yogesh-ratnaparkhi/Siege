using System.Collections.Generic;

namespace Siege.Security.Providers
{
    public interface IConsumerProvider : IProvider<Consumer>
    {
        IList<Consumer> All();
    }
}