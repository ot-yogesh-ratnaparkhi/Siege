using System;
using System.Collections;

namespace Siege.ServiceLocation
{
    public class InstanceUseCase<TBaseType> : UseCase<TBaseType, TBaseType>
    {
        protected TBaseType implementation;

        public virtual void BindTo(TBaseType implementation)
        {
            this.implementation = implementation;
        }

        public override TBaseType GetBinding()
        {
            return this.implementation;
        }

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new ImplementationActivationStrategy(implementation);
        }

        public override Type GetBoundType()
        {
            return this.implementation.GetType();
        }

        public class ImplementationActivationStrategy : IActivationStrategy
        {
            private readonly TBaseType implementation;

            public ImplementationActivationStrategy(TBaseType implementation)
            {
                this.implementation = implementation;
            }

            public TBaseType Resolve(IServiceLocator locator, IDictionary constructorArguments)
            {
                return implementation;
            }
        }
    }
}