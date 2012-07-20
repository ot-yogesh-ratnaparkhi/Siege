include Siege::ServiceLocator::UnitTests::TestClasses
include Siege::ServiceLocator::UnitTests

Given ITestInterface
    When TestContext do |x| x.TestCases == TestEnum.Case2 end
    Then TestCase2.new