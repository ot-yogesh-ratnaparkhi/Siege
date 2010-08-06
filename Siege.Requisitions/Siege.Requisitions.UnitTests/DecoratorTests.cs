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

using NUnit.Framework;
using Siege.Requisitions.UnitTests.RegistrationExtensions.Classes;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;

namespace Siege.Requisitions.UnitTests
{
    public abstract partial class ServiceLocatorTests
    {
        [Test]
        public void ShouldDecorateWithAllConditionsMet()
        {
            locator
                .Register(Given<ICoffee>.Then<Coffee>())
                .Register(Given<ICoffee>
                              .When<Ingredients>(ingredients => ingredients == Ingredients.WhippedCream)
                              .DecorateWith(coffee => new WhippedCreamDecorator(coffee)))
                .Register(Given<ICoffee>
                              .When<Ingredients>(ingredients => ingredients == Ingredients.Espresso)
                              .DecorateWith(coffee => new EspressoShotDecorator(coffee)));

            locator.AddContext(Ingredients.WhippedCream);
            locator.AddContext(Ingredients.Espresso);

            var instance = locator.GetInstance<ICoffee>();

            Assert.AreEqual(1.5M, instance.Total);
        }

        [Test]
        public void ShouldDecorateWithSomeConditionsMet()
        {
            locator
                .Register(Given<ICoffee>.Then<Coffee>())
                .Register(Given<ICoffee>
                              .When<Ingredients>(ingredients => ingredients == Ingredients.WhippedCream)
                              .DecorateWith(coffee => new WhippedCreamDecorator(coffee)))
                .Register(Given<ICoffee>
                              .When<Ingredients>(ingredients => ingredients == Ingredients.Espresso)
                              .DecorateWith(coffee => new EspressoShotDecorator(coffee)));

            locator.AddContext(Ingredients.WhippedCream);

            var instance = locator.GetInstance<ICoffee>();

            Assert.AreEqual(1M, instance.Total);
        }

        [Test]
        public void ShouldDecorateWithNoConditionsMet()
        {
            locator
                .Register(Given<ICoffee>.Then<Coffee>())
                .Register(Given<ICoffee>
                              .When<Ingredients>(ingredients => ingredients == Ingredients.WhippedCream)
                              .DecorateWith(coffee => new WhippedCreamDecorator(coffee)))
                .Register(Given<ICoffee>
                              .When<Ingredients>(ingredients => ingredients == Ingredients.Espresso)
                              .DecorateWith(coffee => new EspressoShotDecorator(coffee)));

            var instance = locator.GetInstance<ICoffee>();

            Assert.AreEqual(0.75M, instance.Total);
        }

        [Test]
        public void ShouldDecorateAsDefault()
        {
            locator
                .Register(Given<ICoffee>.Then<Coffee>())
                .Register(Given<ICoffee>.DecorateWith(coffee => new WhippedCreamDecorator(coffee)))
                .Register(Given<ICoffee>.DecorateWith(coffee => new EspressoShotDecorator(coffee)));

            var instance = locator.GetInstance<ICoffee>();

            Assert.AreEqual(1.5M, instance.Total);
        }

        [Test]
        public void ShouldDecorateNestedDependencies()
        {
            locator
                .Register(Given<CoffeeWrapper>.Then<CoffeeWrapper>())
                .Register(Given<ICoffee>.Then<Coffee>())
                .Register(Given<ICoffee>.DecorateWith(coffee => new WhippedCreamDecorator(coffee)))
                .Register(Given<ICoffee>.DecorateWith(coffee => new EspressoShotDecorator(coffee)));

            var instance = locator.GetInstance<CoffeeWrapper>();

            Assert.AreEqual(1.5M, instance.Coffee.Total);
        }
    }

    public class CoffeeWrapper
    {
        public ICoffee Coffee { get; set; }

        public CoffeeWrapper(ICoffee coffee)
        {
            this.Coffee = coffee;
        }
    }
}