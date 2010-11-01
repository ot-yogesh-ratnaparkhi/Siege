using System.ServiceModel;
using Siege.Courier.Messages;

namespace Siege.Courier.WCF
{
    [ServiceContract]
    public interface IWCFProtocol
    {
        [OperationContract]
        [NetDataContract]
        IMessage Send(IMessage message);
    }
}