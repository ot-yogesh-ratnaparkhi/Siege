using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Siege.Integration.WCF
{
    public class WCFChannelManagerFactory : IChannelManagerFactory
    {
        private static readonly object lockObject = new object();
        private static readonly Dictionary<Type, IChannelFactory> factories = new Dictionary<Type, IChannelFactory>();

        public IChannelManager<TService> Create<TService>(string endPointName)
        {
            ChannelFactory<TService> factory = null;

            if (factories.ContainsKey(typeof(ChannelFactory<TService>))) factory = factories[typeof(ChannelFactory<TService>)] as ChannelFactory<TService>;

            if (factory == null)
            {
                lock (lockObject)
                {
                    if (factories.ContainsKey(typeof(ChannelFactory<TService>))) factory = factories[typeof(ChannelFactory<TService>)] as ChannelFactory<TService>;

                    if (factory == null)
                    {
                        factory = new ChannelFactory<TService>(endPointName);
                        factories.Add(typeof (ChannelFactory<TService>), factory);
                    }
                }
            }

            return new WCFChannelManager<TService>(factory);
        }
    }
}