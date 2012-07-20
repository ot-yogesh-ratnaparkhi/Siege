namespace Siege.Integration.WCF
{
    public interface IChannelManager<TService>
    {
        TService Open();
        void Close(TService service);
    }
}