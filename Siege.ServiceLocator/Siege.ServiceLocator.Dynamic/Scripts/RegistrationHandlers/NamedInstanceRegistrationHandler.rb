class NamedInstanceRegistrationHandler

    def Handle(component)
        x = Given[component.base].Then(component.name, component.type)
        x
    end

end