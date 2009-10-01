using System;
using System.Collections;
using Siege.ServiceLocation.TypeGeneration;

namespace Siege.ServiceLocation
{
    public class GenericUseCase<TBaseType> : UseCase<TBaseType, Type>
    {
        protected Type boundType;

        public void BindTo<TImplementationType>()
        {
            boundType = TypeGenerator.Generate<TImplementationType>();
        }

        public override Type GetBinding()
        {
            return boundType;
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new GenericActivationStrategy(boundType);
        }

        public override Type GetBoundType()
        {
            return boundType;
        }

        public class GenericActivationStrategy : IActivationStrategy
        {
            private readonly Type boundType;

            public GenericActivationStrategy(Type boundType)
            {
                this.boundType = boundType;
            }

            public TBaseType Resolve(IServiceLocator locator, IDictionary constructorArguments)
            {
                return locator.GetInstance<TBaseType>(boundType, constructorArguments);
            }
        }
    }
}