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
using Siege.Requisitions.Registrations.Stores;
using Siege.Requisitions.RegistrationTemplates;
using Siege.Requisitions.RegistrationTemplates.OpenGenerics;

namespace Siege.Requisitions.Registrations.OpenGenerics
{
    public class OpenGenericRegistration : Registration, IOpenGenericRegistration
    {
        private readonly Type mapsFromType;
        protected Type mapsToType;
        private readonly OpenGenericRegistrationTemplate openGenericRegistrationTemplate = new OpenGenericRegistrationTemplate();
        private readonly OpenGenericRegistrationStore openGenericRegistrationStore = new OpenGenericRegistrationStore();

        public OpenGenericRegistration(Type mapsFromType)
        {
            if (!mapsFromType.IsGenericType) throw new Exception("Type: " + mapsFromType + " is not a generic type.");
            this.mapsFromType = mapsFromType;
        }

        public override void MapsTo(object mapsToType)
        {
            this.mapsToType = (Type)mapsToType;
        }

        public override object GetMappedTo()
        {
            return GetMappedToType();
        }

        public override Type GetMappedToType()
        {
            return mapsToType;
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            throw new NotImplementedException();
        }

        public override IRegistrationStore GetRegistrationStore()
        {
            return openGenericRegistrationStore;
        }

        public override IRegistrationTemplate GetRegistrationTemplate()
        {
            return openGenericRegistrationTemplate;
        }

        public override Type GetMappedFromType()
        {
            return mapsFromType;
        }

        IOpenGenericRegistration IOpenGenericRegistration.Then(Type type)
        {
            if (!type.IsGenericType) throw new Exception("Type: " + type + " is not a generic type.");

            MapsTo(type);

            return this;
        }
    }
}