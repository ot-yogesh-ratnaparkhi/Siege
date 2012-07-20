class RubyImpl
    include Siege::ServiceLocator::UnitTests::TestClasses::ITestInterface
    
    attr_reader :TestProperty
    
    def TestProperty= (value)
        @TestProperty = value
    end
end