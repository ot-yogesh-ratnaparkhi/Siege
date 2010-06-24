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
using Siege.ServiceLocation.Resolution;
using Siege.ServiceLocation.UseCases;

namespace Siege.ServiceLocation
{
    public interface IServiceLocator : Microsoft.Practices.ServiceLocation.IServiceLocator, IDisposable, IInstanceResolver, IFactoryFetcher
    {
        IServiceLocator Register(List<IUseCase> useCase);
        IServiceLocator Register(IUseCase useCase);
        new object GetInstance(Type type);
        new object GetInstance(Type type, string key);
        TService GetInstance<TService>(Type type, params IResolutionArgument[] arguments);
        TService GetInstance<TService>(string key, params IResolutionArgument[] arguments);
        TService GetInstance<TService>(params IResolutionArgument[] arguments);
    }
}