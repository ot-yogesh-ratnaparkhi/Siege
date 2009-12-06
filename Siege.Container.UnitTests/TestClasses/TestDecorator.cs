namespace Siege.Container.UnitTests.TestClasses
{
    public class TestDecorator : ITestInterface
    {
        private readonly ITestInterface wrappedObject;

        public TestDecorator(ITestInterface wrappedObject)
        {
            this.wrappedObject = wrappedObject;
        }

        public ITestInterface WrappedObject
        {
            get { return wrappedObject; }
        }
    }
}
