/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Web;
using Siege.Requisitions.InternalStorage;

namespace Siege.Requisitions.HttpIntegration
{
    public class HttpSessionStore : IContextStore
    {
        public void Add(object contextItem)
        {
            if (SessionExists())
            {
                HttpContext.Current.Session.Add(contextItem.GetType() + Guid.NewGuid().ToString(), contextItem);
            }
            else
            {
                HttpContext.Current.Application.Add(contextItem.GetType() + Guid.NewGuid().ToString(), contextItem);
            }
        }

        public List<object> Items
        {
            get
            {
                var items = new List<object>();

                if (SessionExists())
                {
                    foreach (string item in HttpContext.Current.Session)
                    {
                        items.Add(HttpContext.Current.Session[item]);
                    }
                }
                foreach (string item in HttpContext.Current.Application)
                {
                    items.Add(HttpContext.Current.Application[item]);
                }

                return items;
            }
        }

        private static bool SessionExists()
        {
            return HttpContext.Current.Session != null;
        }

        public void Clear()
        {
            if (SessionExists())
                HttpContext.Current.Session.Clear();
            HttpContext.Current.Application.Clear();
        }

        public void Dispose()
        {

        }
    }
}
