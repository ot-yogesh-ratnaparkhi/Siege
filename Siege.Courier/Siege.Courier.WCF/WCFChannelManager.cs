using System.ServiceModel;

namespace Siege.Courier.WCF
{
    public class WCFChannelManager<TService> : IChannelManager<TService>
    {
        private readonly ChannelFactory<TService> channelFactory;

        public WCFChannelManager(ChannelFactory<TService> channelFactory)
        {
            this.channelFactory = channelFactory;
        }

        public TService Open()
        {
            TService service = channelFactory.CreateChannel();
            ((IClientChannel) service).Open();

            return service;
        }

        public void Close(TService service)
        {
            var channel = (ICommunicationObject) service;
            if (channel.State == CommunicationState.Opened)
            {
                channel.Close();
            }
            else
            {
                channel.Abort();
            }
        }
    }
}