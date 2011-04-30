include Siege::Requisitions::Registrations::Default
include Siege::Requisitions::Extensions::ExtendedRegistrationSyntax
include Siege::Requisitions::UnitTests::TestClasses

class Installer

    attr_reader :registrations
    attr_reader :instance
    
    def self.instance
 	    @@instance ||= new
 	end
    
    def initialize()
        @registrations = []
    end
    
    def last_registration
        @registrations.last
    end
    
    def add_registration(type)
        @registrations << RubyRegistration.new(type)
    end
    
    def Install()
      instances = []
        
      @@instance.registrations.each do |component|
        x = Given[component.base].method(:Then).of(component.type).call()
        
        if(component.scope != nil)
            policy = component.scope.new()
            policy.Handle x
            instances << policy
        else
            instances << x
        end
        
 	  end
 	    
      instances
    end

end

class RubyRegistration

    attr_reader :base
    attr_reader :type
    attr_reader :scope

    def initialize(base)
        @base = base
    end
    
    def map_to(type)
        @type = type
    end
    
    def set_scope(scope)
        @scope = scope
    end

end