class ConditionalRegistrationHandler

    def Handle(component)
        rule = ConditionalActivationRule.of(component.base).new
        evaluation = LambdaCondition.of(component.conditiontype).new component.condition
        rule.set_evaluation evaluation
        x = rule.method(:Then).of(component.type).call()
        
        x
    end

end