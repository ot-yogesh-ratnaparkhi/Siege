class DefaultInstanceRegistrationHandler

    def Handle(component)
        x = Given[component.base].Then component.type
        x
    end

end