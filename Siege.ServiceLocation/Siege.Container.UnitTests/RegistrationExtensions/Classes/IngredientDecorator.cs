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

namespace Siege.ServiceLocation.UnitTests.RegistrationExtensions.Classes
{
    public abstract class IngredientDecorator : ICoffee
    {
        protected ICoffee _decoratedCoffee;

        protected IngredientDecorator(ICoffee decoratedCoffee)
        {
            _decoratedCoffee = decoratedCoffee;
        }

        public virtual string Name
        {
            get { return _decoratedCoffee.Name; }
        }

        public virtual decimal Total
        {
            get { return _decoratedCoffee.Total; }
        }
    }
}
