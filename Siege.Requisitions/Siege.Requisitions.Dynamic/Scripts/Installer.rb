include Siege::Requisitions::Registrations::Default
include Siege::Requisitions::Extensions::ExtendedRegistrationSyntax
include System

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
        
        x = RegistrationHandlerFactory.Create component

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