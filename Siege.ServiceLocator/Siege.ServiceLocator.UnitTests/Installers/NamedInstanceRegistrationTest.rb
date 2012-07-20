include Siege::ServiceLocator::UnitTests::TestClasses
include Siege::ServiceLocator::UnitTests

Given ITestInterface 
    Then TestCase2.new
    Named "Test"

Given ITestInterface
    Then TestCase1.new
    Named "Test1"