using System;

namespace Siege.Courier.WCF
{
    public class WCFProxy<TService>
    {
        private readonly IChannelManager<TService> channelManager;

        public WCFProxy(IChannelManager<TService> channelManager)
        {
            this.channelManager = channelManager;
        }

        public TResponse Perform<TResponse>(Func<TService, TResponse> action)
        {
            TService service = channelManager.Open();
            TResponse response;
            try
            {
                response = action.Invoke(service);
            }
            finally
            {
                channelManager.Close(service);
            }

            return response;
        }

        public void Perform(Action<TService> action)
        {
            TService service = channelManager.Open();
            try
            {
                action.Invoke(service);
            }
            finally
            {
                channelManager.Close(service);
            }
        }
    }
}