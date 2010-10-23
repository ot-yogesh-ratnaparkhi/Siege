using System;

namespace Siege.Courier
{
    public class SimpleServiceBus : IServiceBus
    {
        public void Publish(IMessage message)
        {
            throw new NotImplementedException();
        }
    }
}