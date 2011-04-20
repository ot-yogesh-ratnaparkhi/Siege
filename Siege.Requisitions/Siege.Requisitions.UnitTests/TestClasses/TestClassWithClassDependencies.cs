namespace Siege.Requisitions.UnitTests.TestClasses
{
    public class TestClassWithClassDependencies
    {
        public TestClassWithInterfaceDependencies TestClassDependices { get; set; }

        public TestClassWithClassDependencies(TestClassWithInterfaceDependencies testClassDependices)
        {
            TestClassDependices = testClassDependices;
        }
    }
}
