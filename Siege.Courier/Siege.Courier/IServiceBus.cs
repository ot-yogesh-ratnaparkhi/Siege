namespace Siege.Courier
{
    public interface IServiceBus
    {
        void Publish(IMessage message);
    }
}