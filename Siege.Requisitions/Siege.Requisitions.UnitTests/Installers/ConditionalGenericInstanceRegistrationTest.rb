include Siege::Requisitions::Registrations
include Siege::Requisitions::UnitTests::TestClasses
include Siege::Requisitions::UnitTests
include Siege::Requisitions::RegistrationPolicies
include Siege::Requisitions::ResolutionRules

Given TestCase4
    Then TestCase4

Given IConstructorArgument
    Then ConstructorArgument

Given ITestInterface
    When TestEvaluation
    Then TestCase2.new