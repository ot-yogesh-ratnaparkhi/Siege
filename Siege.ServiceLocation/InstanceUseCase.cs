using System;
using System.Collections;

namespace Siege.ServiceLocation
{
    public class InstanceUseCase<TBaseService> : UseCase<TBaseService, TBaseService>
    {
        protected TBaseService implementation;

        public virtual void BindTo(TBaseService implementation)
        {
            this.implementation = implementation;
        }

        public override TBaseService GetBinding()
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
            private readonly TBaseService implementation;

            public ImplementationActivationStrategy(TBaseService implementation)
            {
                this.implementation = implementation;
            }

            public TBaseService Resolve(IServiceLocator locator, IDictionary constructorArguments)
            {
                return implementation;
            }
        }
    }
}