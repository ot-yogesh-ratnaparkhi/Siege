include Siege::ServiceLocator::Registrations
include Siege::ServiceLocator::UnitTests::TestClasses
include Siege::ServiceLocator::UnitTests
include Siege::ServiceLocator::RegistrationPolicies
include Siege::ServiceLocator::ResolutionRules

Given ITestInterface 
    Then TestCase2.new
    Named "Test"

Given ITestInterface
    Then TestCase1.new
    Named "Test1"