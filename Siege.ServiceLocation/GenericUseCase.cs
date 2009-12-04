using System;

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

        protected override IActivationStrategy<TBaseService> GetActivationStrategy()
        {
            return new GenericActivationStrategy(boundType);
        }

        public override Type GetBoundType()
        {
            return boundType;
        }

        public class GenericActivationStrategy : IActivationStrategy<TBaseService>
        {
            private readonly Type boundType;

            public GenericActivationStrategy(Type boundType)
            {
                this.boundType = boundType;
            }

            public TBaseService Resolve(IInstanceResolver locator)
            {
                return (TBaseService)locator.GetInstance(boundType);
            }
        }
    }
}