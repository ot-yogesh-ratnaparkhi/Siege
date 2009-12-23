namespace Siege.ServiceLocation.UnitTests.ContextualTests.Classes
{
    public interface ITestCondition
    {
        TestTypes TestType { get; set; }
    }

    public enum TestTypes
    {
        Test3, 
        Test1,
        Test2
    }
}
