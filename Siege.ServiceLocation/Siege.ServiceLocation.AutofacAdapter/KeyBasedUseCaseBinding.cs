using Autofac;
using Autofac.Builder;

namespace Siege.ServiceLocation.AutofacAdapter
{
    public class KeyBasedUseCaseBinding<TService> : IKeyBasedUseCaseBinding<TService>
    {
        private IContainer container;

        public KeyBasedUseCaseBinding(IContainer container)
        {
            this.container = container;
        }

        public void Bind(IUseCase useCase)
        {
            Bind((IKeyBasedUseCase<TService>)useCase);
        }

        public void Bind(IKeyBasedUseCase<TService> useCase)
        {
            var builder = new ContainerBuilder();

            builder.Register(useCase.GetBoundType()).Named(useCase.Key).FactoryScoped();

            builder.Build(container);
        }
    }
}