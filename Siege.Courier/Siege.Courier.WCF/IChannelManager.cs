namespace Siege.Courier.WCF
{
    public interface IChannelManager<TService>
    {
        TService Open();
        void Close(TService service);
    }
}