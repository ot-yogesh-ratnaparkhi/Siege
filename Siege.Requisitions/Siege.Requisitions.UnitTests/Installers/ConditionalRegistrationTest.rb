include Siege::Requisitions::Registrations
include Siege::Requisitions::UnitTests::TestClasses
include Siege::Requisitions::UnitTests
include Siege::Requisitions::RegistrationPolicies
include Siege::Requisitions::ResolutionRules

Given ITestInterface
    When TestContext do |x| x.TestCases == TestEnum.Case2 end
    Then TestCase2