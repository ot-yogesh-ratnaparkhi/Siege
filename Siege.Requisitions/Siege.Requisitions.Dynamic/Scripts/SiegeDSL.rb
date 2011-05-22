def Given (base) 
    Installer.instance.add_registration base
    Installer.instance.last_registration.set_default_or_conditional "Default"
end

def Scope (scope)
    Installer.instance.last_registration.set_scope scope
end
    
def Then (type)
    if(type.is_a? Class)
        Installer.instance.last_registration.map_to type
        Installer.instance.last_registration.set_registration_type "Type"
    else
        Installer.instance.last_registration.map_to type
        Installer.instance.last_registration.set_registration_type "Instance"
    end
end

def When(type, &condition)
    Installer.instance.last_registration.set_condition_type type, condition
    Installer.instance.last_registration.set_default_or_conditional "Conditional" 
end

def Condition(&condition)
    Installer.instance.last_registration.set_condition condition
end
