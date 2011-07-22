class DefaultRegistrationHandler

    def Handle(component)
        x = Given[component.base].method(:Then).of(component.type).call()
        x
    end

end