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
using System.Linq;
using System.Web;
using Siege.ServiceLocator.InternalStorage;
using Siege.ServiceLocator.Resolution;

namespace Siege.ServiceLocator.Web
{
    public class HttpResolutionStore : IResolutionStore
    {
        public HttpResolutionStore()
        {
            this.Clear();
        }

        public void Add(List<IResolutionArgument> contextItem)
        {
            HttpContext.Current.Items.Add("Resolution" + Guid.NewGuid(), contextItem);
        }

        public void Clear()
        {
            var list = HttpContext.Current.Items.Keys.Cast<object>().Where(key => key.ToString().StartsWith("Resolution")).ToList();

            list.ForEach(k => HttpContext.Current.Items.Remove(k));
        }

        public List<IResolutionArgument> Items
        {
            get
            {
                var items = new List<IResolutionArgument>();

                foreach (object keyObject in HttpContext.Current.Items.Keys)
                {
                    var key = keyObject as string;
                    if (!String.IsNullOrEmpty(key) && key.StartsWith("Resolution"))
                        items.AddRange((List<IResolutionArgument>)HttpContext.Current.Items[key]);
                }

                return items;
            }
        }

        public void Dispose()
        {
            this.Clear();
        }
    }
}