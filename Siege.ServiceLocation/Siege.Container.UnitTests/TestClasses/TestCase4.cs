namespace Siege.Container.UnitTests.TestClasses
{
    public class TestCase4 : ITestInterface
    {
        private readonly IConstructorArgument argument;

        public TestCase4(IConstructorArgument argument)
        {
            this.argument = argument;
        }

        public IConstructorArgument Argument
        {
            get { return argument; }
        }
    }
}