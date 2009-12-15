using System;
using System.Collections.Generic;

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

        protected override IActivationStrategy GetActivationStrategy()
        {
            return new ImplementationActivationStrategy(implementation);
        }

        public override Type GetBoundType()
        {
            return implementation.GetType();
        }

        public override Type GetBaseBindingType()
        {
            return typeof (TBaseService);
        }

        public class ImplementationActivationStrategy : IActivationStrategy
        {
            private readonly TBaseService implementation;

            public ImplementationActivationStrategy(TBaseService implementation)
            {
                this.implementation = implementation;
            }

            public object Resolve(IInstanceResolver locator, IList<object> context)
            {
                return implementation;
            }
        }
    }
}