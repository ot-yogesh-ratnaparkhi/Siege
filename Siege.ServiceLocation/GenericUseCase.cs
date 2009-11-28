using System;
using System.Collections;

namespace Siege.ServiceLocation
{
    public abstract class GenericUseCase<TBaseService> : UseCase<TBaseService, Type>
    {
        protected Type boundType;

        public void BindTo<TImplementationType>()
        {
            boundType = typeof(TImplementationType);
        }

        public void BindTo(Type type)
        {
            boundType = type;
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

            public TBaseService Resolve(IMinimalServiceLocator locator, IDictionary constructorArguments)
            {
                return (TBaseService)locator.GetInstance(boundType, constructorArguments);
            }
        }
    }
}