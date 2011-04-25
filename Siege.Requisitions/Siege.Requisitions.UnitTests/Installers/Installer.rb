require 'singleton'
include Siege::Requisitions::RegistrationPolicies
include Siege::Requisitions::RegistrationSyntax
include Siege::Requisitions::Registrations

class Component
    attr_reader :scope, :base, :type
    
    def initialize(type = System::Type)
        @type = type
    end
    
    def set_scope(scope = System::Type)
        @scope = scope
    end
    
    def inherit_from(base = System::Type)
        @base = base
    end
end

class Installer
    attr_accessor :Current

    def initialize
        @components = []
    end
        
    def self.instance
        @@instance ||= new
    end
    
    def add_component(component)
        @components << component
    end
    
    def last_component
        @components.last
    end    
    
    def Build()
        instances = System::Collections::Generic::List.of(IRegistration).new()
        @components.each do |component|
            x = Given[TestCase1].method(:Then).of(TestCase1).call()
            instances.add x
        end
        
        instances
    end
end

class ClrInstaller
  def get_Current()
    Installer.instance
  end
end