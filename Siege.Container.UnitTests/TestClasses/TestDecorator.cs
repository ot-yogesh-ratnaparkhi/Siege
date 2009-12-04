namespace Siege.Container.UnitTests.TestClasses
{
    public class TestDecorator : TestCase1
    {
        private readonly TestCase1 wrappedObject;

        public TestDecorator(TestCase1 wrappedObject)
        {
            this.wrappedObject = wrappedObject;
        }

        public TestCase1 WrappedObject
        {
            get { return wrappedObject; }
        }
    }
}
