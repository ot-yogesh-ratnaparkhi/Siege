using NUnit.Framework;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.UnitTests.TestClasses;
using TestContext = Siege.Requisitions.UnitTests.TestClasses.TestContext;

namespace Siege.Requisitions.UnitTests
{
    public abstract partial class ServiceLocatorTests
    {
        [Test]
        public void ShouldBeAbleToBindAnInterfaceToATypeBasedOnMultipleConditionalRules()
        {
            locator
                .Register(Given<ITestInterface>
                            .When(context =>
                                      {
                                          context.When<TestEnum>(testenum => testenum == TestEnum.Case2);
                                          context.When<TestEnum>(testenum => testenum == TestEnum.Case1);
                                      })
                            .Then<TestCase2>());
            locator.AddContext(TestEnum.Case2);
            locator.AddContext(TestEnum.Case1);

            Assert.IsInstanceOf<TestCase2>(locator.GetInstance<ITestInterface>());
        }
    }
}