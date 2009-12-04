using System;

namespace Siege.ServiceLocation
{
    public abstract class InstanceUseCase<TBaseService> : UseCase<TBaseService, TBaseService>
    {
        protected TBaseService implementation;

        public virtual void BindTo(TBaseService implementation)
        {
            this.implementation = implementation;
        }

        public override TBaseService GetBinding()
        {
            return implementation;
        }

        protected override IActivationStrategy<TBaseService> GetActivationStrategy()
        {
            return new ImplementationActivationStrategy(implementation);
        }

        public override Type GetBoundType()
        {
            return implementation.GetType();
        }

        public class ImplementationActivationStrategy : IActivationStrategy<TBaseService>
        {
            private readonly TBaseService implementation;

            public ImplementationActivationStrategy(TBaseService implementation)
            {
                this.implementation = implementation;
            }

            public TBaseService Resolve(IInstanceResolver locator)
            {
                return implementation;
            }
        }
    }
}