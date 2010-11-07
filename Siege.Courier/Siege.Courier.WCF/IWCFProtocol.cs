using System.Collections.Generic;
using System.ServiceModel;
using Siege.Courier.Messages;

namespace Siege.Courier.WCF
{
    [ServiceContract]
    public interface IWCFProtocol
    {
        [OperationContract]
        [NetDataContract]
        List<IMessage> Send(IMessage message);
    }
}