include Siege::ServiceLocator::Registrations
include Siege::ServiceLocator::UnitTests::TestClasses
include Siege::ServiceLocator::UnitTests
include Siege::ServiceLocator::RegistrationPolicies
include Siege::ServiceLocator::ResolutionRules

Given TestCase4
    Then TestCase4

Given IConstructorArgument
    Then ConstructorArgument

Given ITestInterface
    When TestEvaluation
    Then TestCase2.new