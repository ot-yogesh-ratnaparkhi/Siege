require 'Siege.Requisitions.UnitTests.dll'
require 'Siege.Requisitions.dll'
require 'Siege.Requisitions.Extensions.dll'
require 'Installers\SiegeDSL'
include Siege::Requisitions::Registrations
include Siege::Requisitions::UnitTests::TestClasses
include Siege::Requisitions::UnitTests

Component TestCase1
#    Scope Siege::Requisitions::RegistrationPolicies::Singleton
#    Inherits Siege::Requisitions::UnitTests::TestClasses::ITestInterface