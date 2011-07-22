namespace Siege.Integration.WCF
{
    public interface IChannelManagerFactory
    {
        IChannelManager<TService> Create<TService>(string endPointName);
    }
}