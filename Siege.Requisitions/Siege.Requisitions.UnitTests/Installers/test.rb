require 'Installers\SiegeDSL'
require 'Installers\Installer'
include Siege::Requisitions::Registrations
include Siege::Requisitions::UnitTests::TestClasses
include Siege::Requisitions::UnitTests
include Siege::Requisitions::RegistrationPolicies

Given ITestInterface 
Then TestCase1
Scope Singleton