require 'Installers\Installer'

def Given (base) 
    Installer.instance.add_registration base
end

def Scope (scope)
    Installer.instance.last_registration.set_scope scope
end
    
def Then (type)
    Installer.instance.last_registration.map_to type
end

