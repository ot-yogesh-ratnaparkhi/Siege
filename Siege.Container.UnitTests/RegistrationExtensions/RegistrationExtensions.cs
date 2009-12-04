using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.RegistrationExtensions
{
    public static class RegistrationExtensions
    {
        public static IDecoratorUseCase<TService> DecorateWith<TService, TDecorator>(this IActivationRule useCase)
        {
            var decoratorUseCase = new DecoratorUseCase<TService>();

            decoratorUseCase.BindTo<TDecorator>();
            decoratorUseCase.AddActivationRule(useCase);
            
            return decoratorUseCase;
        }
    }
}
