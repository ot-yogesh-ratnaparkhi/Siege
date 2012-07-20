class ConventionInstanceRegistrationHandler

    def Handle(component)
        x = Using.Convention component.type
        x
    end

end