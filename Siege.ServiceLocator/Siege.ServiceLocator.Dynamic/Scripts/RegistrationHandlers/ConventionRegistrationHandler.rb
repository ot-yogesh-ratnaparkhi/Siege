class ConventionRegistrationHandler

    def Handle(component)
        x = Using.method(:Convention).of(component.type).call()
		x
    end

end