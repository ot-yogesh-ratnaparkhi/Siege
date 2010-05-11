using NUnit.Framework;
using Siege.ServiceLocation.Extensions.ExtendedSyntax;
using Siege.ServiceLocation.UnitTests.RegistrationExtensions.Classes;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Should_Decorate_With_All_Conditions_Met()
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
        public void Should_Decorate_With_Some_Conditions_Met()
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
        public void Should_Decorate_With_No_Conditions_Met()
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

    }
}