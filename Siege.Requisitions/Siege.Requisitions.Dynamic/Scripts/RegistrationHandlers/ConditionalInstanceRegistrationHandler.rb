class ConditionalInstanceRegistrationHandler

    def Handle(component)
        rule = ConditionalActivationRule.of(component.base).new
        evaluation = LambdaCondition.of(component.conditiontype).new component.condition
        rule.set_evaluation evaluation
        x = rule.Then component.type
        
        x
    end

end