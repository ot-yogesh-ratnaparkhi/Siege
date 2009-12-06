namespace Siege.Container.UnitTests.TestClasses
{
    public class TestContext
    {
        public TestContext(TestEnum context)
        {
            TestCases = context;
        }

        public TestEnum TestCases { get; set; }
    }
}
