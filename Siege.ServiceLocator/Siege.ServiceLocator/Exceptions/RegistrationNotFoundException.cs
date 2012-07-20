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

namespace Siege.ServiceLocator.Exceptions
{
    public class RegistrationNotFoundException : ApplicationException
    {
        public RegistrationNotFoundException(Type type)
            : base("Type not registered: " + type)
        {
        }

        public RegistrationNotFoundException(Type type, Exception innerException)
            : base("Type not registered: " + type, innerException)
        {
        }

        public RegistrationNotFoundException(Type type, string key)
            : base("Type not registered: " + type + " with key: " + key)
        {
        }

        public RegistrationNotFoundException(Type type, string key, Exception innerException)
            : base("Type not registered: " + type + " with key: " + key, innerException)
        {
        }
    }
}
