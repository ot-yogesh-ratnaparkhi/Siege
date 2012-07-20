include Siege::ServiceLocator::UnitTests::TestClasses
include Siege::ServiceLocator::UnitTests

Given ITestInterface 
    Then TestCase2
    Named "Test"

Given ITestInterface
    Then TestCase1
    Named "Test1"