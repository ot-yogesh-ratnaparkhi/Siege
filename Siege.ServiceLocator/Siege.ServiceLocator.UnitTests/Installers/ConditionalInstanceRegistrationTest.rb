include Siege::ServiceLocator::Registrations
include Siege::ServiceLocator::UnitTests::TestClasses
include Siege::ServiceLocator::UnitTests
include Siege::ServiceLocator::RegistrationPolicies
include Siege::ServiceLocator::ResolutionRules

Given ITestInterface
    When TestContext do |x| x.TestCases == TestEnum.Case2 end
    Then TestCase2.new