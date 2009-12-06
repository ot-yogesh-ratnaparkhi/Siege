namespace Siege.Container.UnitTests.ContextualTests.Classes
{
    public class TestCondition : ITestCondition
    {
        public TestTypes TestType { get; set; }

        public TestCondition(TestTypes test)
        {
            TestType = test;
        }
    }
}
