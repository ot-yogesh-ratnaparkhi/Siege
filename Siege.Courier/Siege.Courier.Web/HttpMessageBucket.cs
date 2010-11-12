using System.Collections.Generic;
using System.Web;
using Siege.Courier.Messages;

namespace Siege.Courier.Web
{
    public class HttpMessageBucket : IMessageBucket
    {
        public void Add(IMessage message)
        {
            var messages = (List<IMessage>) HttpContext.Current.Items["bucket"];
            messages = messages ?? new List<IMessage>();

            messages.Add(message);

            HttpContext.Current.Items["bucket"] = messages;
        }

        public List<IMessage> All()
        {
            var messages = (List<IMessage>) HttpContext.Current.Items["bucket"];
            messages = messages ?? new List<IMessage>();

            return messages;
        }

        public void Clear()
        {
            HttpContext.Current.Items["bucket"] = new List<IMessage>();
        }
    }
}