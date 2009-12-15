using System;
using System.Collections.Generic;
using System.Web;

namespace Siege.ServiceLocation.HttpIntegration
{
    public class HttpSessionStore : IContextStore
    {
        public void Add(object contextItem)
        {
            HttpContext.Current.Session.Add(contextItem.GetType() + Guid.NewGuid().ToString(), contextItem);
        }

        public List<object> Items
        {
            get
            {
                List<object> items = new List<object>();

                foreach(string item in HttpContext.Current.Session)
                {
                    items.Add(HttpContext.Current.Session[item]);
                }

                return items;
            }
        }
    }
}
