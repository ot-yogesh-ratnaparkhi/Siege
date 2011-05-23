include Siege::Requisitions::Registrations::Default
include Siege::Requisitions::Extensions::ExtendedRegistrationSyntax
include System
include System::Collections::Generic

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

        if (x.is_a? IList.of(IRegistration))
            x.each do |item|
                instances << GetRegistration(component, item)
            end
        else
            instances << GetRegistration(component, x)
        end
        
 	  end
 	    
      instances
    end

    def GetRegistration(component, x)
        if(component.scope != nil)
            policy = component.scope.new()
            policy.Handle x
            policy
        else
            x
        end
    end

end