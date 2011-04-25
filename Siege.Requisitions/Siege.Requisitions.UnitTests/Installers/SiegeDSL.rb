require 'Installers\Installer'

def Component (type) 
    Installer.instance.add_component type
end

def Scope (scope)
    Installer.instance.last_component.set_scope scope
end
    
def Inherits (base)
    Installer.instance.last_component.inherit_from base
end

