include Siege::ServiceLocator::Registrations
include Siege::ServiceLocator::UnitTests::TestClasses
include Siege::ServiceLocator::UnitTests
include Siege::ServiceLocator::RegistrationPolicies
include Siege::ServiceLocator::ResolutionRules

require 'Installers/RubyImpl'

test = RubyImpl.new
test.TestProperty= 3

Given ITestInterface
    Then test