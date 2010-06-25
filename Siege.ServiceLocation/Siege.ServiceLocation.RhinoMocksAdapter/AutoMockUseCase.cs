using System;
using Siege.ServiceLocation.Bindings.Default;
using Siege.ServiceLocation.Stores;
using Siege.ServiceLocation.UseCases;
using Siege.ServiceLocation.UseCases.Default;

namespace Siege.ServiceLocation.RhinoMocksAdapter
{
    public class AutoMockUseCase : UseCase, IInstanceUseCase, IDefaultUseCase
    {
        private readonly Type from;
        protected object implementation;

        public AutoMockUseCase(Type from, object to)
        {
            this.from = from;
            this.implementation = to;
        }

        object IInstanceUseCase.GetBinding()
        {
            return implementation;
        }

        public override Type GetUseCaseBindingType()
        {
            return typeof (DefaultUseCaseBinding);
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
            return from;
        }

        public class ImplementationActivationStrategy : IActivationStrategy
        {
            private readonly object implementation;

            public ImplementationActivationStrategy(object implementation)
            {
                this.implementation = implementation;
            }

            public object Resolve(IInstanceResolver locator, IServiceLocatorStore context)
            {
                return implementation;
            }
        }
    }
}