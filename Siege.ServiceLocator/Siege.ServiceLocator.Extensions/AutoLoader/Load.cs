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
using System.IO;
using System.Linq;
using System.Reflection;

namespace Siege.ServiceLocator.Extensions.AutoLoader
{
    public class Load
    {
        public Action<IServiceLocator> FromAssembliesIn(string folder, List<Assembly> registeredTypeAssemblies)
        {
            return locator =>
            {
                var files = Directory.GetFiles(folder).ToList();
                files.ForEach(file =>
                {
                    if(!file.EndsWith(".dll")) return;
                    var types = Assembly.LoadFrom(file).GetTypes().ToList();
                    types.ForEach(type =>
                    {
                        if(type.GetInterfaces().Contains(typeof(IAutoloader)))
                        {
                            var instance = (IAutoloader)type.GetConstructor(new Type[] {}).Invoke(new object[] {});
                            locator.Register(instance.Load(locator, registeredTypeAssemblies));
                        }
                    });
                });
            };
        }
    }
}