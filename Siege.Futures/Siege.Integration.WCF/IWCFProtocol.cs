using System.Collections.Generic;
using System.ServiceModel;
using Siege.Eventing.Messages;

namespace Siege.Integration.WCF
{
    [ServiceContract]
    public interface IWCFProtocol
    {
        [OperationContract]
        [NetDataContract]
        List<IMessage> Send(IMessage message);
    }
}