class NamedRegistrationHandler

    def Handle(component)
        x = Given[component.base].method(:Then).of(component.type).call(component.name)
        x
    end

end