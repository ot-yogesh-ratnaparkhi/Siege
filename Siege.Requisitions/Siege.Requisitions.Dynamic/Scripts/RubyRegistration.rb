class RubyRegistration

    attr_reader :base
    attr_reader :type
    attr_reader :scope
    attr_reader :condition
    attr_reader :conditiontype
    attr_reader :name

    def initialize(base)
        @base = base
    end
    
    def map_to(type)
        @type = type
    end
    
    def set_scope(scope)
        @scope = scope
    end
    
    def set_condition_type(conditiontype, condition)
        @conditiontype = conditiontype 
        @condition = condition
    end
    
    def set_condition(condition)
        @condition = condition
    end

    def set_default_or_conditional(value)
        @default_or_conditional = value
    end

    def set_registration_type(type)
        @registration_type = type
    end

    def registrationType
        @default_or_conditional + @registration_type
    end

    def set_name(name)
        @name = name
    end
end