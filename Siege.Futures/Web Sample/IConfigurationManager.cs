namespace Web.Sample
{
    public interface IConfigurationManager
    {
        string ServiceBusEndPoint { get; }
    }

    public class ServiceBusConfigurationManager : IConfigurationManager
    {
        public string ServiceBusEndPoint
        {
            get { return "WSHttpBinding_IWCFProtocol"; }
        }
    }
}