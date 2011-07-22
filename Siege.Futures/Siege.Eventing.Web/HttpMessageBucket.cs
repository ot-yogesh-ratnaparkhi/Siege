using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Siege.Eventing;
using Siege.Eventing.Messages;

namespace Siege.Eventing.Web
{
    public class HttpMessageBucket : IMessageBucket
    {
        private readonly TempDataDictionary tempData;

        public HttpMessageBucket(TempDataDictionary tempData)
        {
            this.tempData = tempData;
        }

        public void Add(IMessage message)
        {
            var messages = (List<IMessage>) tempData["bucket"];
            messages = messages ?? new List<IMessage>();

            messages.Add(message);

            tempData["bucket"] = messages;
        }

        public List<IMessage> All()
        {
            var messages = (List<IMessage>) tempData["bucket"];
            messages = messages ?? new List<IMessage>();

            return messages;
        }

        public void Clear()
        {
            tempData["bucket"] = new List<IMessage>();
        }
    }
}