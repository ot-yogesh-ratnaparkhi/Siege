using NUnit.Framework;
using Rhino.Mocks;
using Siege.ServiceLocation.RhinoMocksAdapter;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
    public partial class SiegeContainerTests
    {
        [Test]
        public void Should_AutoMock_All_Dependencies()
        {
            var repository = new MockRepository();

            locator.Register(AutoMock<DependsOnMultipleInterfaceTypes>.Using(repository));

            Assert.IsNotNull(locator.GetInstance<IConstructorArgument>());
            Assert.IsNotNull(locator.GetInstance<IServiceLocator>());
            Assert.IsNotNull(locator.GetInstance<DependsOnMultipleInterfaceTypes>());
        }

        [Test]
        public void Should_AutoStub_All_Dependencies()
        {
            var repository = new MockRepository();

            locator.Register(AutoMock<MultiConstructorType>.Using(repository));

            Assert.IsNotNull(locator.GetInstance<TypeA>());
            Assert.IsNotNull(locator.GetInstance<TypeB>());
            Assert.IsNotNull(locator.GetInstance<TypeC>());
            Assert.IsNotNull(locator.GetInstance<TypeD>());
            Assert.IsNotNull(locator.GetInstance<TypeE>());
            Assert.IsNotNull(locator.GetInstance<MultiConstructorType>());
        }
    }
}