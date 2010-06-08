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
using Siege.ServiceLocation.Bindings.OpenGenerics;

namespace Siege.ServiceLocation.UseCases.OpenGenerics
{
    public class OpenGenericUseCase : UntypedUseCase, IOpenGenericUseCase
    {
        private readonly Type baseBinding;
        protected Type boundType;

        public OpenGenericUseCase(Type baseBinding)
        {
            if (!baseBinding.IsGenericType) throw new Exception("Type: " + baseBinding + " is not a generic type.");
            this.baseBinding = baseBinding;
        }

        public void BindTo(Type implementationType)
        {
            boundType = implementationType;
        }

        public override Type GetBoundType()
        {
            return boundType;
        }

        public override Type GetUseCaseBindingType()
        {
            return typeof (OpenGenericUseCaseBinding);
        }

        public override Type GetBaseBindingType()
        {
            return baseBinding;
        }

        IOpenGenericUseCase IOpenGenericUseCase.Then(Type type)
        {
            if (!type.IsGenericType) throw new Exception("Type: " + type + " is not a generic type.");

            BindTo(type);

            return this;
        }
    }
}