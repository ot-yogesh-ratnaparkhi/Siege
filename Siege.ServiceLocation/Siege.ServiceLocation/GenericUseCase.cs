using System;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public abstract class GenericUseCase<TBaseService> : UseCase<TBaseService, Type>
    {
        protected Type boundType;

        public void BindTo<TImplementationType>()
        {
            BindTo(typeof(TImplementationType));
        }

        public void BindTo(Type implementationType)
        {
            boundType = implementationType;
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

            public object Resolve(IInstanceResolver locator, IList<object> context)
            {
                return locator.GetInstance(boundType);
            }
        }
    }
}