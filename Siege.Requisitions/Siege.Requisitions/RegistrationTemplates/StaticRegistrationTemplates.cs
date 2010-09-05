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

using Siege.Requisitions.RegistrationTemplates.Conditional;
using Siege.Requisitions.RegistrationTemplates.Default;
using Siege.Requisitions.RegistrationTemplates.Named;
using Siege.Requisitions.RegistrationTemplates.OpenGenerics;
using Siege.Requisitions.RegistrationTemplates.PostResolution;

namespace Siege.Requisitions.RegistrationTemplates
{
    //this exists for performance purposes only
    public class StaticRegistrationTemplates
    {
        public static IRegistrationTemplate DefaultRegistrationTemplate { get; private set; }
        public static IRegistrationTemplate DefaultInstanceRegistrationTemplate { get; private set; }
        public static IRegistrationTemplate ConditionalRegistrationTemplate { get; private set; }
        public static IRegistrationTemplate ConditionalInstanceRegistrationTemplate { get; private set; }
        public static IRegistrationTemplate NamedRegistrationTemplate { get; private set; }
        public static IRegistrationTemplate NamedInstanceRegistrationTemplate { get; private set; }
        public static IRegistrationTemplate OpenGenericRegistrationTemplate { get; private set; }
        public static IRegistrationTemplate ConditionalPostResolutionRegistrationTemplate { get; private set; }
        public static IRegistrationTemplate DefaultPostResolutionRegistrationTemplate { get; private set; }

        static StaticRegistrationTemplates()
        {
            DefaultRegistrationTemplate = new DefaultRegistrationTemplate();
            DefaultInstanceRegistrationTemplate = new DefaultInstanceRegistrationTemplate();
            ConditionalRegistrationTemplate = new ConditionalRegistrationTemplate();
            ConditionalInstanceRegistrationTemplate = new ConditionalInstanceRegistrationTemplate();
            NamedRegistrationTemplate = new NamedRegistrationTemplate();
            NamedInstanceRegistrationTemplate = new NamedInstanceRegistrationTemplate();
            OpenGenericRegistrationTemplate = new OpenGenericRegistrationTemplate();
            ConditionalPostResolutionRegistrationTemplate = new ConditionalPostResolutionRegistrationTemplate();
            DefaultPostResolutionRegistrationTemplate = new DefaultPostResolutionRegistrationTemplate();
        }
    }
}