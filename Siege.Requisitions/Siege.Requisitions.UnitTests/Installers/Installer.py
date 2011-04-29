import clr
clr.AddReference("Siege.Requisitions")
clr.AddReference("Siege.Requisitions.Extensions")
clr.AddReference("Siege.Requisitions.UnitTests")
from Siege.Requisitions.Registrations.Default import *
from Siege.Requisitions.Extensions.ExtendedRegistrationSyntax import *
from Siege.Requisitions.UnitTests.TestClasses import *

class Installer:
    def Install(self):
        x = DefaultRegistration[ITestInterface]()
        x.MapsTo[TestCase1]()
        return x

Factory.Create(Installer)
