class RegistrationHandlerFactory

    def self.Create(rubyRegistration)
        
        if (rubyRegistration.registrationType == "DefaultType")
            handler = DefaultRegistrationHandler.new()
        elsif (rubyRegistration.registrationType == "DefaultInstance")
            handler = DefaultInstanceRegistrationHandler.new()
        elsif (rubyRegistration.registrationType == "ConditionalType")
            handler = ConditionalRegistrationHandler.new()
        elsif (rubyRegistration.registrationType == "ConditionalInstance")
            handler = ConditionalInstanceRegistrationHandler.new()
        end

        result = handler.Handle rubyRegistration
        result
    end

end