namespace Siege.Courier.WCF
{
    public interface IChannelManagerFactory
    {
        IChannelManager<TService> Create<TService>(string endPointName);
    }
}