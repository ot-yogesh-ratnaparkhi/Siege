namespace Siege.Requisitions.UnitTests.TestClasses
{
    public class TestClassWithInterfaceDependencies
    {
        public TestInterfaceWithMethods TestInterface { get; set; }
        public TestClassWithInterfaceDependencies(TestInterfaceWithMethods testInterface)
        {
            TestInterface = testInterface;
        }

        public virtual int GetSomeValue(int i)
        {
            return i;
        }
    }
}
