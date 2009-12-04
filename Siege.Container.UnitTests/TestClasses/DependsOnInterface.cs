namespace Siege.Container.UnitTests.TestClasses
{
    public class DependsOnInterface : ITestInterface
    {
        private readonly IConstructorArgument argument;

        public DependsOnInterface(IConstructorArgument argument)
        {
            this.argument = argument;
        }

        public IConstructorArgument Argument
        {
            get { return argument; }
        }
    }
}