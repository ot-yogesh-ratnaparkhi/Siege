using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Siege.Courier.WCF
{
    public class WCFChannelManagerFactory : IChannelManagerFactory
    {
        private static readonly object lockObject = new object();
        private static readonly Dictionary<Type, IChannelFactory> factories = new Dictionary<Type, IChannelFactory>();

        public IChannelManager<TService> Create<TService>(ChannelConfig config)
        {
            var factory = factories[typeof (ChannelFactory<TService>)] as ChannelFactory<TService>;

            if (factory == null)
            {
                lock (lockObject)
                {
                    factory = factories[typeof (ChannelFactory<TService>)] as ChannelFactory<TService>;
                    if (factory == null)
                    {
                        factory = new ChannelFactory<TService>(config.EndPointConfigurationName);
                        factories.Add(typeof (ChannelFactory<TService>), factory);
                    }
                }
            }

            return new WCFChannelManager<TService>(factory);
        }
    }
}